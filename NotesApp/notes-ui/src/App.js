import React from 'react';
import RegisterForm from './components/registerForm.js';
import LoginForm from './components/loginForm.js';
import ForgotPassword from './components/forgotPassword.js';
import Navbar from './components/navbar.js';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './app.css';

function App() {
  return (
    <div className='app'>
      <BrowserRouter>
        <Navbar/>
        <Routes>
          <Route path='login' element={<LoginForm/>}/>
          <Route path='register' element={<RegisterForm/>}/>
          <Route path='forgot-password' element={<ForgotPassword/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
