import React, { useState } from 'react';
import RegisterForm from './components/registerForm.js';
import LoginForm from './components/loginForm.js';
import ForgotPassword from './components/forgotPassword.js';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import NotFound from './components/notFound.js';
import ResetPassword from './components/resetPassword.js';
import MainLayout from './components/layouts/mainLayout.js';
import UserNotes from './components/userNotes.js';
import LoginMessageContext from './services/loginMessageContext';
import './app.css';

function App() {
  const [loginMessage, setLoginMessage] = useState('');
  const [isLoginMsgError, setIsLoginMsgError] = useState(false);

  return (
    <div className='app'>
      <LoginMessageContext.Provider value={{loginMessage, setLoginMessage, isLoginMsgError, setIsLoginMsgError}}>
        <BrowserRouter>
          <Routes>
            <Route path='/accounts/' element={<MainLayout/>}>
              <Route path='' element={<Navigate to='/login' replace/>}/>
              <Route path='login' element={<LoginForm/>}/>
              <Route path='register' element={<RegisterForm/>}/>
              <Route path='forgot-password' element={<ForgotPassword/>}/>
              <Route path='reset-password/:id' element={<ResetPassword/>}/>
            </Route>
            <Route path='/notes' element={<MainLayout/>}>
              <Route path='' element={<UserNotes/>}/>
            </Route>
            <Route path='/not-found' element={<NotFound/>} />            
            <Route path='*' element={<Navigate to='/notes' replace/>}/>
          </Routes>
        </BrowserRouter>
      </LoginMessageContext.Provider>
    </div>
  );
}

export default App;
