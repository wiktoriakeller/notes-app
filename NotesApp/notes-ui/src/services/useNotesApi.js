import { useNavigate } from "react-router-dom";
import { useRef } from "react";

function useNotesApi() {
    const baseUri = 'https://localhost:7164';
    const registerPath = '/notes-api/accounts/register';
    const loginPath = '/notes-api/accounts/login';
    const forgotPasswordPath = '/notes-api/accounts/forgot-password';
    const resetPasswordPath = '/notes-api/accounts/reset-password';
    const getAllNotesPath = '/notes-api/notes';
    const notFoundPath = '/not-found';

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
    const user = useRef(null);

    const register = async (data) => {
        return await fetchData(registerPath, data, 'POST');
    };

    const login = async(data) => {
        user.current = null;
        localStorage.removeItem('user');
        let result = await fetchData(loginPath, data, 'POST');

        if(result.success === true) {
            let jwt = await result.response.text();
            localStorage.setItem('user', jwt);
            user.current = jwt;
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
        if(user.current === null)
            logout();

        let result = await fetchData(getAllNotesPath, {}, 'GET');
        if(result.success === true) {
            let data = await result.response.json();
            result.data = data;
        }
    
        return result;
    }

    const logout = () => {
        user.current = null;
        localStorage.removeItem('user');
        navigate(clientPaths['login']);
    }

    const isUserLogged = () => {
        if(user.current !== null)
            return true;

        return false;
    };

    const fetchData = async (path, data, method) => {
        try {
            let user = localStorage.getItem('user');
            let jwtToken = user === null ? '' : user; 

            const fetchData = {
                method: method,
                headers: {
                    'Content-type' : 'application/json',
                    'Authorization': `Bearer ${jwtToken}`
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
                        navigate(clientPaths['login'], { state: { msg:'Invalid request', isError: true } });
                    }
                    else if(response.status === StatusCodes.Status401 || response.status == StatusCodes.Status403){
                        logout();
                    }
                    else if(response.status === StatusCodes.Status404) {
                        navigate(clientPaths['not-found']); 
                    }
                    else if(response.status === StatusCodes.Status500) {
                        navigate(clientPaths['login'], { state: { msg:'Internal server error', isError: true } });
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
                navigate(clientPaths['login'], { state: { msg:'Server is down', isError: true } });
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
            getAllNotes
        }
    )
}

export default useNotesApi;
