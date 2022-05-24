import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import LoginMessageContext from '../services/loginMessageContext';

function useNotesApi() {
    const baseUri = 'https://localhost:7164';
    const registerPath = '/notes-api/accounts/register';
    const loginPath = '/notes-api/accounts/login';
    const forgotPasswordPath = '/notes-api/accounts/forgot-password';
    const resetPasswordPath = '/notes-api/accounts/reset-password';
    const getAllNotesPath = '/notes-api/notes';
    const postNotePath = '/notes-api/notes';
    const getPublicLinkPath = '/notes-api/notes/';
    const getPublicNotePath = '/notes-api/notes/public/';
    const editNotePath = '/notes-api/notes';
    const deleteNotePath = '/notes-api/notes/';

    const clientPaths = {
        'login': '/accounts/login',
        'register': '/accounts/register',
        'forgot-password': '/accounts/forgot-password',
        'reset-password': '/accounts/reset-password',
        'not-found': '/not-found'
    };
    
    const StatusCodes = {
        Status400: 400,
        Status401: 401,
        Status403: 403,
        Status404: 404,
        Status500: 500
    };

    const navigate = useNavigate();
    const {loginMessage, setLoginMessage, isLoginMsgError, setIsLoginMsgError} = useContext(LoginMessageContext);

    const register = async (data) => {
        return await fetchData(registerPath, data, 'POST');
    };

    const login = async(data) => {
        localStorage.removeItem('user');
        const result = await fetchData(loginPath, data, 'POST');

        if(result.success === true) {
            const jwt = await result.response.text();
            localStorage.setItem('user', jwt);
        }
    
        return result;
    }

    const forgotPassword = async (data) => {
        return await fetchData(forgotPasswordPath, data, 'POST');
    }
    
    const resetPassword = async (data, token) => {
        return await fetchData(resetPasswordPath + '/' + token, data, 'POST');
    }
    
    const getAllNotes = async () => {
        const user = getUser();
        if(user === '') {
            logout();
        }

        const result = await fetchData(getAllNotesPath, {}, 'GET');
        if(result.success === true) {
            let data = await result.response.json();
            result.data = data;
        }
    
        return result;
    }

    const postNote = async (data) => {
        const user = getUser();
        if(user === '') {
            logout();
        }

        const result = await fetchData(postNotePath, data, 'POST');
        return result;
    }

    const editNote = async (data) => {
        const user = getUser();
        if(user === '') {
            logout();
        }

        const result = await fetchData(editNotePath, data, 'PUT');
        return result;
    }

    const deleteNote = async (hashid) => {
        const user = getUser();
        if(user === '') {
            logout();
        }

        const result = await fetchData(deleteNotePath + hashid, {}, 'DELETE');
        return result;
    }

    const generatePublicLink = async (data, hashid) => {
        const user = getUser();
        if(user === '') {
            logout();
        }

        const result = await fetchData(getPublicLinkPath + hashid, data, 'PATCH');
        if(result.success === true && !data.resetPublicHashId) {
            let data = await result.response.json();
            result.data = data;
        }

        return result;
    }

    const getPublicNote = async (hashid) => {
        const result = await fetchData(getPublicNotePath + hashid, {}, 'GET');
        if(result.success === true) {
            let data = await result.response.json();
            result.data = data;
        }

        return result;
    }

    const logout = () => {
        localStorage.removeItem('user');
        setLoginMessage('');
        setIsLoginMsgError(false);
        navigate(clientPaths['login']);
    }

    const isUserLogged = () => {
        let user = getUser();
        if(user !== '')
            return true;

        return false;
    }

    const getUser = () => {
        let user = localStorage.getItem('user');
        let jwtToken = user === null ? '' : user; 
        return jwtToken;
    }

    const fetchData = async (path, data, method) => {
        try {
            const user = getUser();

            const fetchData = {
                method: method,
                headers: {
                    'Content-type' : 'application/json',
                    'Authorization': `Bearer ${user}`
                }
            };

            if(method === 'POST' || method === 'PUT' || method === 'PATCH') {
                fetchData.body = JSON.stringify(data);            
            }

            const response = await fetch(baseUri + path, fetchData);

            if(response.ok === true) {
                return {success: true, errors: {}, response: response};
            }
            else {
                try {
                    let errors = JSON.parse(await response.text()).errors;
                    return {success: false, errors: errors};
                }
                catch(e) {
                    if(response.status === StatusCodes.Status400) {
                        setLoginMessage('Invalid request');
                        setIsLoginMsgError(true);
                        navigate(clientPaths['login']);
                    }
                    else if(response.status === StatusCodes.Status401 || response.status == StatusCodes.Status403){
                        logout();
                    }
                    else if(response.status === StatusCodes.Status404) {
                        navigate(clientPaths['not-found']); 
                    }
                    else if(response.status === StatusCodes.Status500) {
                        localStorage.removeItem('user');
                        setLoginMessage('Internal server error');
                        setIsLoginMsgError(true);
                        navigate(clientPaths['login']);
                    }

                    return {success: false, errors: {}};
                }
            }
        }
        catch(e) {
            const location = window.location.pathname;
            if(location === clientPaths['login'] || location === clientPaths['register'] 
                || location === clientPaths['forgot-password']) {
                return {success: false, errors: {'serverError': 'Server is down'}};
            }
            else {
                localStorage.removeItem('user');
                setLoginMessage('Server is down');
                setIsLoginMsgError(true);
                navigate(clientPaths['login']);
            }
        }
    }

    return (
        {
            login,
            register,
            logout,
            isUserLogged,
            forgotPassword,
            resetPassword,
            getAllNotes,
            generatePublicLink,
            deleteNote,
            getPublicNote,
            editNote,
            postNote
        }
    )
}

export default useNotesApi;
