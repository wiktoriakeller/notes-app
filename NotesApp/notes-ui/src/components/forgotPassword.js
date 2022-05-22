import React from 'react';
import { useState, useEffect, useContext } from 'react';
import useNotesApi from '../services/useNotesApi';
import { Link, useNavigate } from 'react-router-dom';
import LoginMessageContext from '../services/loginMessageContext';
import InputForm from './inputForm';
import './styles/registerForm.css';
import './styles/forgotPassword.css';

const emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const ForgotPassword = () => {
  const [email, setEmail] = useState('');
  const [isEmailValid, setIsEmailValid] = useState(false);
  const [emailFocus, setEmailFocus] = useState(false);
  const emailErrorMsg = 'Email should be valid.';

  const [errorMsg, setErrorMsg] = useState([]);
  const [showErrors, setShowErrors] = useState(false)
  const [disableButton, setDisableButton] = useState(false);  

  const navigate = useNavigate();
  const notesApi = useNotesApi();
  const {loginMessage, setLoginMessage, isLoginMsgError, setIsLoginMsgError} = useContext(LoginMessageContext);

  useEffect(() => {
      setIsEmailValid(emailRegex.test(email));
      setShowErrors(false);
  }, [email]);

  const handleSubmit = async (e) => {
      e.preventDefault();
      setErrorMsg([]);
      setShowErrors(false);
      setDisableButton(true);

      let data = {
        'email': email
      };
  
      let response = await notesApi.forgotPassword(data);

      if(response.success === true) {
        setLoginMessage('Email has been sent!');
        setIsLoginMsgError(false);
        navigate("/accounts/login");
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
      <form className='inner-form' id='forgot-password-form' onSubmit={handleSubmit}>
        <>
        {errorMsg.map((msg) => {
          return <p className={showErrors ? 'error' : 'hide'}>{msg}</p>;
        })}
        <div className='info-title'>Trouble with logging in?</div><br /><div className='info'>Enter your email address and we'll send you a link to get back into your account.</div>
          <InputForm
            label='Email'
            name='email'
            type='email'
            value={email}
            errorMessage={emailErrorMsg}
            isValid={isEmailValid}
            isFocused={emailFocus}
            onChange={(e) => setEmail(e.target.value)}
            onFocus={() => setEmailFocus(true)} />
          <button type='submit' disabled={!isEmailValid || disableButton}>Send Link</button>
        </>
        <p className='account-info'>
          <Link to='/accounts/login'>Back</Link>
        </p>
      </form>
    </div>)
}

export default ForgotPassword