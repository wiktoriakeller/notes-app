const baseUri = 'https://localhost:7164';
const registerPath = '/notes-api/accounts/register';
const loginPath = '/notes-api/accounts/login';
const forgotPasswordPath = '/notes-api/accounts/forgot-password';
const resetPasswordPath = '/notes-api/accounts/reset-password';

const getAllNotesPath = '/notes-api/notes/';

const StatusCodes = {
    Status400: 400,
    Status401: 401,
    Status403: 403,
    Status404: 404,
    Status500: 500
};

async function register(data, navigate) {
    return await fetchData(registerPath, data, 'POST', navigate);
}

async function login(data, navigate) {
    let result = await fetchData(loginPath, data, 'POST', navigate);
    if(result.success === true) {
        let jwt = await result.response.text();
        result.jwt = jwt;
    }

    return result;
}

async function forgotPassword(data, navigate) {
    return await fetchData(forgotPasswordPath, data, 'POST', navigate);
}

async function resetPassword(data, token, navigate) {
    return await fetchData(resetPasswordPath + '/' + token, data, 'POST', navigate);
}

async function getAllNotes(jwtToken, navigate) {
    let result = await fetchData(getAllNotesPath, {}, 'GET', navigate, jwtToken);
    
    if(result.success === true) {
        let data = await result.response.json();
        result.data = data;
    }

    return result;
}

async function fetchData(path, data, method, navigate, jwtToken = '') {
    try {
        const fetchData = {
            method: method,
            headers: {
                'Content-type' : 'application/json',
                'Authorization': `Bearer ${jwtToken}`
            }
        };

        if(method === 'POST') {
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
                    navigate('/accounts/login', { state: { msg:'Invalid request', isError: true } });
                }
                else if(response.status === StatusCodes.Status401 || response.status == StatusCodes.Status403){
                    //navigate('/accounts/login', { state: { msg:'You are unauthorized', isError: true } });
                    navigate('/accounts/login');
                }
                else if(response.status === StatusCodes.Status404) {
                    navigate('/not-found'); 
                }
                else if(response.status === StatusCodes.Status500) {
                    navigate('/accounts/login', { state: { msg:'Internal server error', isError: true } });
                }

                return {success: false, errors: {}};
            }
        }
    }
    catch(e) {
        return {success: false, errors: {'serverError': 'Server is down'}};
    }
}

export { register, login as signIn, forgotPassword, resetPassword, getAllNotes };