import React from 'react';
import RegisterForm from './components/registerForm.js';
import LoginForm from './components/loginForm.js';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './app.css';

function App() {
  return (
    <div className='app'>
      <BrowserRouter>
        <Routes>
          <Route path='login' element={<LoginForm/>}/>
          <Route path='register' element={<RegisterForm/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
