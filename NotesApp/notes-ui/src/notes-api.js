const baseUri = 'https://localhost:7164';

async function register(data) {
    const path = '/notes-api/accounts/register';
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
        let errors = JSON.parse(await response.text()).errors;
        return {success: false, errors: errors};
    }
}

export default register;