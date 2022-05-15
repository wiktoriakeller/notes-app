const baseUri = 'https://localhost:7164';
const registerPath = '/notes-api/accounts/register';
const loginPath = '/notes-api/accounts/login';
const forgotPasswordPath = '/notes-api/accounts/forgot-password';

async function register(data) {
    try {
        let response = await fetchData(registerPath, data);
    
        if(response.ok === true) {
            return {success: true, errors: {}};
        }
        else {
            let errors = JSON.parse(await response.text()).errors;
            return {success: false, errors: errors};
        }
    }
    catch(e) {
        return {success: false, errors: {'serverError': 'Server is down'}};
    }
}

async function login(data) {
    try {
        let response = await fetchData(loginPath, data);
    
        if(response.ok === true) {
            return {success: true, errors: {}};
        }
        else {
            return {success: false, errors: {'Invalid credentials': 'Login or password is incorrect'}};
        }
    }
    catch(e) {
        return {success: false, errors: {'serverError': 'Server is down'}};
    }
}

async function forgotPassword(data) {
    try {
        let response = await fetchData(forgotPasswordPath, data);
    
        if(response.ok === true) {
            return {success: true, errors: {}};
        }
        else {
            let errors = JSON.parse(await response.text()).errors;
            return {success: false, errors: errors};
        }
    }
    catch(e) {
        return {success: false, errors: {'serverError': 'Server is down'}};
    }
}

async function fetchData(path, data) {
    const response = await fetch(baseUri + path, 
    {
        method: 'POST',
        headers: {
            'Content-type' : 'application/json'
        },
        body: JSON.stringify(data)
    });

    return response;
}

export { register, login as signIn, forgotPassword };