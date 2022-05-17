import React from 'react';
import { useState, useEffect } from 'react';
import { forgotPassword } from '../notes-api';
import { Link } from 'react-router-dom';
import InputForm from './inputForm';
import './styles/registerForm.css';
import './styles/forgotPassword.css';

const emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const ForgotPassword = () => {
  const [email, setEmail] = useState('');
  const [isEmailValid, setIsEmailValid] = useState(false);
  const [emailFocus, setEmailFocus] = useState(false);
  const emailErrorMsg = 'Email should be valid.';

  const [errorMsg, setErrorMsg] = useState([])
  const [sendEmail, setSendEmail] = useState(false)
  const [disableButton, setDisableButton] = useState(false);  

  useEffect(() => {
      setIsEmailValid(emailRegex.test(email));
  }, [email]);

  const handleSubmit = async (e) => {
      e.preventDefault();
      setErrorMsg([]);
      setDisableButton(true);

      let data = {
        'email': email
      };
  
      let response = await forgotPassword(data);
      if(response.success === true) {
        setSendEmail(true);
      }
      else {
        setSendEmail(false);
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
        {errorMsg.map((msg) => {
          return <p className={sendEmail ? 'hide' : 'error'}>{msg}</p>;
        })}
        {!sendEmail ?
          <><div className='info-title'>Trouble with logging in?</div><br /><div className='info'>Enter your email address and we'll send you a link to get back into your account.</div>
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
            <button type='submit' disabled={!isEmailValid || disableButton}>
              Send Link
            </button></>
          :
          <p className='success-msg'>An email has been successfully send!</p>}
        <p className='account-info'>
          <Link to='/login'>Back</Link>
        </p>
      </form>
    </div>)
}

export default ForgotPassword