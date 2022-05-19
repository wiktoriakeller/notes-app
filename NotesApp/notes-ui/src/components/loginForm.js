import React from 'react';
import { useState, useEffect } from 'react';
import useNotesApi from '../services/useNotesApi';
import { Link, useLocation, useNavigate } from 'react-router-dom';
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
    const [success, setSuccess] = useState(true)
    const [disableButton, setDisableButton] = useState(false);  

    const [showMessage, setShowMessage] = useState(false);
    const [isMsgError, setIsMsgError] = useState(false);
    const [message, setMessage] = useState('');
    const {state} = useLocation();

    const navigate = useNavigate();
    const notesApi = useNotesApi();

    useEffect(() => {
        if (state !== undefined && state !== null) {
            setShowMessage(true);     
            const {msg, isError} = state;
            setMessage(msg);
            setIsMsgError(isError);
        }
    }, []);

    useEffect(() => {
        setIsLoginValid(login !== '');
        if (login != '') {
            setShowMessage(false);
            setMessage('');
        }
    }, [login]);

    useEffect(() => {
        setIsPasswordValid(password != '');
        if(password != '') {
            setShowMessage(false);
            setMessage('');
        }
    }, [password]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMsg([]);
        setDisableButton(true);

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
          setSuccess(false);
          setErrorMsg(errorMessages);
        }

        setDisableButton(false);
    };

    return (
        <div className='register-form'>
            <form className='inner-form' onSubmit={handleSubmit}>
                {errorMsg.map((msg) => {
                    return <p className={success ? 'hide' : 'error'}>{msg}</p>;
                })}
                <p className={showMessage && !isMsgError ? 'message-info' : 'hide' }>{message}</p>
                <p className={showMessage && isMsgError ? 'error-info' : 'hide' }>{message}</p>
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