import React from 'react';
import RegisterForm from './components/registerForm.js';
import LoginForm from './components/loginForm.js';
import ForgotPassword from './components/forgotPassword.js';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import NotFound from './components/notFound.js';
import MainLayout from './components/layouts/mainLayout.js';
import './app.css';

function App() {
  return (
    <div className='app'>
      <BrowserRouter>
        <Routes>
          <Route path='login' element={<MainLayout><LoginForm/></MainLayout>}/>
          <Route path='register' element={<MainLayout><RegisterForm/></MainLayout>}/>
          <Route path='forgot-password' element={<MainLayout><ForgotPassword/></MainLayout>}/>
          <Route path='*' element={<NotFound/>} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
