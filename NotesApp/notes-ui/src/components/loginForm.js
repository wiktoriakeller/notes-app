import React from 'react';
import { useState, useEffect } from 'react';
import {signIn} from '../notes-api';
import InputForm from './inputForm';
import './registerForm.css';

const LoginForm = () => {
    const [login, setLogin] = useState('');
    const [isLoginValid, setIsLoginValid] = useState(false);
    const [loginFocus, setLoginFocus] = useState(false);
    const loginErrorMsg = 'Login cannot be empty.';

    const [password, setPassword] = useState('');
    const [isPasswordValid, setIsPasswordValid] = useState(false);
    const [passwordFocus, setPasswordFocus] = useState(false);
    const passwordErrorMsg = 'Password cannot be empty.';
  
    const [errorMsg, setErrorMsg] = useState([])
    const [success, setSuccess] = useState(true)
    const [disableButton, setDisableButton] = useState(false);  

    useEffect(() => {
        setIsLoginValid(login !== '');
    }, [login]);

    useEffect(() => {
        setIsPasswordValid(password != '');
    }, [password]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMsg([]);
        setDisableButton(true);

        let data = {
          'login': login,
          'password': password
        };
    
        let response = await signIn(data);
        if(response.success === true) {
          setSuccess(true);
        }
        else {
          setSuccess(false);
          let errorMessages = [];
          for(const [_, value] of Object.entries(response.errors)) {
            errorMessages.push(value);
          }
          setErrorMsg(errorMessages);
        }

        setDisableButton(false);
    };

    return (
    <div className='register-form'>
        <form className='inner-form' onSubmit={handleSubmit}>
            {errorMsg.map((msg) => {
                return <p class={success ? 'hide' : 'error'}>{msg}</p>
            })}
            <h1>Login</h1>
            <InputForm
                label='Login'
                name='login'
                type='text'
                value={login}
                errorMessage={loginErrorMsg}
                isValid={isLoginValid}
                isFocused={loginFocus}
                onChange={(e) => setLogin(e.target.value)}
                onFocus={() => setLoginFocus(true)}
            />  
            <InputForm
                label='Password'
                name='password'
                type='password'
                value={password}
                autoComplete='off'
                errorMessage={passwordErrorMsg}
                isValid={isPasswordValid}
                isFocused={passwordFocus}
                onFocus={() => setPasswordFocus(true)}
                onChange={(e) => setPassword(e.target.value)}
            />  
            <button type='submit' disabled= {!isLoginValid || !isPasswordValid || disableButton }>
                Login
            </button>
            <p className='account-info'>
                Need an account?<br/>
                <a href="#">Sign Up</a>
            </p>
        </form>
    </div>
    )
}

export default LoginForm