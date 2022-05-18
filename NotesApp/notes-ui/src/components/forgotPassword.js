import React from 'react';
import { useState, useEffect } from 'react';
import { forgotPassword } from '../notesApi';
import { Link, useNavigate } from 'react-router-dom';
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
  const [showError, setShowError] = useState(false)
  const [disableButton, setDisableButton] = useState(false);  

  const navigate = useNavigate();

  useEffect(() => {
      setIsEmailValid(emailRegex.test(email));
  }, [email]);

  const handleSubmit = async (e) => {
      e.preventDefault();
      setErrorMsg([]);
      setShowError(false);
      setDisableButton(true);

      let data = {
        'email': email
      };
  
      let response = await forgotPassword(data, navigate);
      if(response.success === true) {
        navigate("/accounts/login", { state: { msg: 'An email has been sent!', isError: false } });
      }
      else {
        setShowError(true);
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
      <form className='inner-form' id='forgot-password-form' onSubmit={handleSubmit}>
        <>
        {errorMsg.map((msg) => {
          return <p className={showError ? 'error' : 'hide'}>{msg}</p>;
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