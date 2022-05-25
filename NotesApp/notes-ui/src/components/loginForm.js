import React from 'react';
import { useState, useEffect, useContext, useRef } from 'react';
import useNotesApi from '../services/useNotesApi';
import LoginMessageContext from '../services/loginMessageContext';
import { Link, useNavigate } from 'react-router-dom';
import InputForm from './inputForm';
import './styles/registerForm.css';

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
    const [disableButton, setDisableButton] = useState(false);  
    const [showErrors, setShowErrors] = useState(false);

    const {loginMessage, setLoginMessage, isLoginMsgError, setIsLoginMsgError} = useContext(LoginMessageContext);

    const navigate = useNavigate();
    const notesApi = useNotesApi();
    const initialLoginCount = useRef(0);
    const initialPasswordCount = useRef(0);
    const previousLogin = useRef(login);
    const previousPassword = useRef(password);

    useEffect(() => {
        setIsLoginValid(login !== '');
        setShowErrors(false);
        if((previousLogin.current === '' && login.length === 1 && initialLoginCount.current > 1) 
            || (login !== '' && initialLoginCount.current > 2)) {
            setLoginMessage('');
        }
        initialLoginCount.current++;
        previousLogin.current = login;
    }, [login]);

    useEffect(() => {
        setIsPasswordValid(password != '');
        setShowErrors(false);
        if((previousPassword.current === '' && password.length === 1 && initialPasswordCount.current > 1) 
            || (password !== '' && initialPasswordCount.current > 2)) {
            setLoginMessage('');
        }
        initialPasswordCount.current++;
        previousPassword.current = password;
    }, [password]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMsg([]);
        setDisableButton(true);
        setShowErrors(false);
        setLoginMessage('');
        setIsLoginMsgError(false);

        let data = {
          'login': login,
          'password': password
        };
    
        let response = await notesApi.login(data);

        if(response.success === true) {
          navigate('/notes');
        }
        else {
          let errorMessages = [];
          for(const [_, value] of Object.entries(response.errors)) {
            errorMessages.push(value);
          }
          setShowErrors(true);
          setErrorMsg(errorMessages);
        }

        setDisableButton(false);
    };

    return (
        <div className='register-form'>
            <form className='inner-form' onSubmit={handleSubmit}>
                {errorMsg.map((msg) => {
                    return <p className={showErrors ? 'error' : 'hide'}>{msg}</p>;
                })}
                <p className={loginMessage.length != 0 && !isLoginMsgError ? 'message-info' : 'hide' }>{loginMessage}</p>
                <p className={loginMessage.length != 0 && isLoginMsgError ? 'error-info' : 'hide' }>{loginMessage}</p>
                <h1>Login</h1>
                <InputForm
                    label='Login'
                    name='login'
                    type='text'
                    value={login}
                    autoComplete='off'
                    errorMessage={loginErrorMsg}
                    isValid={isLoginValid}
                    isFocused={loginFocus}
                    onChange={(e) => setLogin(e.target.value)}
                    onFocus={() => setLoginFocus(true)} />
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
                    onChange={(e) => setPassword(e.target.value)} />
                <button className='submit-button' type='submit' disabled={!isLoginValid || !isPasswordValid || disableButton}>
                    Login
                </button>
                <p className='account-info'>
                    Forgot your password?<br />
                    <Link to='/accounts/forgot-password'>Go here!</Link>
                </p>
            </form>
        </div>
    )
}

export default LoginForm