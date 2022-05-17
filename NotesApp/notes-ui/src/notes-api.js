import { useNavigate } from 'react-router-dom';

const baseUri = 'https://localhost:7164';
const registerPath = '/notes-api/accounts/register';
const loginPath = '/notes-api/accounts/login';
const forgotPasswordPath = '/notes-api/accounts/forgot-password';
const resetPasswordPath = '/notes-api/accounts/reset-password';

const StatusCodes = {
    Status400: 400,
    Status401: 401,
    Status403: 403,
    Status404: 404,
    Status500: 500
};

async function register(data, navigate) {
    return await fetchData(registerPath, data, navigate);
}

async function login(data, navigate) {
    return await fetchData(loginPath, data, navigate);
}

async function forgotPassword(data, navigate) {
    return await fetchData(forgotPasswordPath, data, navigate);
}

async function resetPassword(data, token, navigate) {
    return await fetchData(resetPasswordPath + '/' + token, data, navigate);
}

async function fetchData(path, data, navigate) {
    try {
        const response = await fetch(baseUri + path, 
        {
            method: 'POST',
            headers: {
                'Content-type' : 'application/json'
            },
            body: JSON.stringify(data)
        });

        if(response.ok === true) {
            return {success: true, errors: {}};
        }
        else {
            try {
                let errors = JSON.parse(await response.text()).errors;
                return {success: false, errors: errors};
            }
            catch(e) {
                if(response.status === StatusCodes.Status400) {
                    navigate("/login", { state: { msg:'Invalid request', isError: true } });
                }
                else if(response.status === StatusCodes.Status401 || response.status == StatusCodes.Status403){
                    navigate("/login", { state: { msg:'You are unauthorized', isError: true } });
                }
                else if(response.status === StatusCodes.Status404) {
                    navigate("/not-found"); 
                }
                else if(response.status === StatusCodes.Status500) {
                    navigate("/login", { state: { msg:'Internal server error', isError: true } });
                }
            }
        }
    }
    catch(e) {
        return {success: false, errors: {'serverError': 'Server is down'}};
        //navigate("/login", { state: { msg:'Server is down', isError: true } });
    }
}

export { register, login as signIn, forgotPassword, resetPassword };