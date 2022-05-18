import React, { useState } from 'react';
import RegisterForm from './components/registerForm.js';
import LoginForm from './components/loginForm.js';
import ForgotPassword from './components/forgotPassword.js';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import NotFound from './components/notFound.js';
import ResetPassword from './components/resetPassword.js';
import MainLayout from './components/layouts/mainLayout.js';
import UserNotes from './components/userNotes.js';
import UserContext from './components/userContext.js';
import './app.css';

function App() {
  const [jwtToken, setJwtToken] = useState(null);

  return (
    <div className='app'>
      <UserContext.Provider value={{jwtToken, setJwtToken}}>
        <BrowserRouter>
          <Routes>
            <Route path='/login' element={<MainLayout><LoginForm/></MainLayout>}/>
            <Route path='/register' element={<MainLayout><RegisterForm/></MainLayout>}/>
            <Route path='/forgot-password' element={<MainLayout><ForgotPassword/></MainLayout>}/>
            <Route path="/reset-password/:id" element={<MainLayout><ResetPassword/></MainLayout>}></Route>
            <Route path='/notes' element={<MainLayout><UserNotes/></MainLayout>}/>
            <Route path='/not-found' element={<NotFound/>} />
            <Route path='*' element={<NotFound/>} />
          </Routes>
        </BrowserRouter>
      </UserContext.Provider>
    </div>
  );
}

export default App;
