const baseUri = 'https://localhost:7164';
const registerPath = '/notes-api/accounts/register';
const loginPath = '/notes-api/accounts/login';

async function register(data) {
    try {
        const response = await fetch(baseUri + registerPath, 
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
        const response = await fetch(baseUri + loginPath, 
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
            return {success: false, errors: {'Invalid credentials': 'Login or password is incorrect'}};
        }
    }
    catch(e) {
        return {success: false, errors: {'serverError': 'Server is down'}};
    }
}

export { register, login as signIn };