import React, { useEffect, useState } from 'react'
import { Link, Outlet } from 'react-router-dom';
import './styles/navbar.css'
import useNotesApi from '../services/useNotesApi';

const Navbar = () => {
  const notesApi = useNotesApi();
  const [buttonMsg, setButtonMsg] = useState('Sign In');

  useEffect(() => {
    if(notesApi.isUserLogged()) {
      setButtonMsg('Sign out');
    }
    else {
      setButtonMsg('Sign in');
    }
  });

  return (
    <>
    <nav className='navbar'>
        <span className='title'><Link to='/notes'>NotesHub</Link></span>
        {
          notesApi.isUserLogged() ?
          <Link to='/accounts/login' id='sign-in-button'><button className='navbar-button' onClick={() => notesApi.logout()}>Sign out</button></Link> :
          <Link to='/accounts/login' id='sign-in-button'><button className='navbar-button'>Sign in</button></Link>
        }
        <Link to='/accounts/register' id='sign-up-button'><button className='navbar-button'>Sign Up</button></Link>
    </nav>
    <Outlet />
    </>
  )
}

export default Navbar;