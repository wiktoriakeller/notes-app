import React, { useEffect, useState, useContext } from 'react';
import InputForm from './inputForm.js';
import useNotesApi from '../services/useNotesApi';
import { Link, useNavigate } from 'react-router-dom';
import LoginMessageContext from '../services/loginMessageContext';
import './styles/registerForm.css';

const loginRegex = /^[A-Za-z][A-Za-z0-9-_]{2,19}$/;
const emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%]).{6,20}$/;
const nameRegex = /[A-Za-z]{1,20}/;
const surnameRegex = /[A-Za-z]{1,20}/;

const RegisterForm = () => {
  const [login, setLogin] = useState('');
  const [isLoginValid, setIsLoginValid] = useState(false);
  const [loginFocus, setLoginFocus] = useState(false);
  const loginErrorMsg = 'Login should be 3-20 characters long and must begin with a letter. Lowercase and uppercase letters, numbers and underscores are allowed.';

  const [email, setEmail] = useState('');
  const [isEmailValid, setIsEmailValid] = useState(false);
  const [emailFocus, setEmailFocus] = useState(false);
  const emailErrorMsg = 'Email should be valid.';

  const [name, setName] = useState('');
  const [isNameValid, setIsNameValid] = useState(false);
  const [nameFocus, setNameFocus] = useState(false);
  const nameErrorMsg = 'Name must be 1-20 characters long and can only contain letters.';

  const [surname, setSurname] = useState('');
  const [isSurnameValid, setIsSurnameValid] = useState(false);
  const [surnameFocus, setSurnameFocus] = useState(false);
  const surnameErrorMsg = 'Surname must be 1-20 characters long and can only contain letters.';

  const [password, setPassword] = useState('');
  const [isPasswordValid, setIsPasswordValid] = useState(false);
  const [passwordFocus, setPasswordFocus] = useState(false);
  const passwordErrorMsg = 'Password should be 6-20 characters long, must include uppercase and lowercase letters, a number and a special character.';

  const [confirm, setConfirm] = useState('')
  const [isConfirmValid, setIsConfirmValid] = useState(false);
  const [confirmFocus, setConfirmFocus] = useState(false);
  const confirmErrorMsg = 'Passwords should match';

  const [errorMsg, setErrorMsg] = useState([])
  const [showErrors, setShowErrors] = useState(false)
  const [disableButton, setDisableButton] = useState(false);

  const navigate = useNavigate();
  const notesApi = useNotesApi();
  const {loginMessage, setLoginMessage, isLoginMsgError, setIsLoginMsgError} = useContext(LoginMessageContext);

  useEffect(() => {
    setIsLoginValid(loginRegex.test(login));
    setShowErrors(false);    
  }, [login]);

  useEffect(() => {
    setIsEmailValid(emailRegex.test(email));
    setShowErrors(false);
  }, [email]);

  useEffect(() => {
    setIsNameValid(nameRegex.test(name));
    setShowErrors(false);
  }, [name]);

  useEffect(() => {
    setIsSurnameValid(surnameRegex.test(surname));
    setShowErrors(false);
  }, [surname]);

  useEffect(() => {
    setIsPasswordValid(passwordRegex.test([password]));
    setIsConfirmValid(password === confirm);
    setShowErrors(false);
  }, [password, confirm]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMsg([]);
    setDisableButton(true);
    setShowErrors(false);

    let data = {
      'login': login,
      'email': email,
      'name': name,
      'surname': surname,
      'password': password,
      'confirmPassword': confirm
    };

    let response = await notesApi.register(data);
    if(response.success === true) {
      setLoginMessage('Account registered!');
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
      <form className='inner-form' onSubmit={handleSubmit}>
        {errorMsg.map((msg) => {
          return <p className={showErrors ? 'error' : 'hide'}>{msg}</p>;
        })}
        <h1>Register</h1>
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
          label='Email'
          name='email'
          type='text'
          value={email}
          errorMessage={emailErrorMsg}
          isValid={isEmailValid}
          isFocused={emailFocus}
          onFocus={() => setEmailFocus(true)}
          onChange={(e) => setEmail(e.target.value)} />
        <InputForm
          label='Name'
          name='name'
          type='text'
          value={name}
          errorMessage={nameErrorMsg}
          isValid={isNameValid}
          isFocused={nameFocus}
          onFocus={() => setNameFocus(true)}
          onChange={(e) => setName(e.target.value)} />
        <InputForm
          label='Surname'
          name='surname'
          type='text'
          value={surname}
          errorMessage={surnameErrorMsg}
          isValid={isSurnameValid}
          isFocused={surnameFocus}
          onFocus={() => setSurnameFocus(true)}
          onChange={(e) => setSurname(e.target.value)} />
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
        <InputForm
          label='Confirm password'
          name='confirm'
          type='password'
          value={confirm}
          autoComplete='off'
          errorMessage={confirmErrorMsg}
          isValid={isConfirmValid}
          isFocused={confirmFocus}
          onFocus={() => setConfirmFocus(true)}
          onChange={(e) => setConfirm(e.target.value)} />
        <button type='submit' disabled={!isLoginValid || !isEmailValid || !isNameValid || !isSurnameValid || !isPasswordValid || !isConfirmValid || disableButton}>
          Submit
        </button>
        <p className='account-info'>
          Already registered?<br />
          <Link to='/accounts/login'>Sign In</Link>
        </p>
      </form>
    </div>
  )
}

export default RegisterForm