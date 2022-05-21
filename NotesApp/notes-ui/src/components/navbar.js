import React, { useEffect, useState } from 'react'
import { Link, Outlet } from 'react-router-dom';
import './styles/navbar.css'
import useNotesApi from '../services/useNotesApi';

const Navbar = () => {
  const notesApi = useNotesApi();

  return (
    <>
    <nav className='navbar'>
        <span className='title'><Link to='/notes'>NotesHub</Link></span>
        {
          notesApi.isUserLogged() ?
          <Link to='/accounts/login' id='left-button'><button className='navbar-button' onClick={() => notesApi.logout()}>Sign out</button></Link> :
          <Link to='/accounts/login' id='left-button'><button id='sign-in-button' className='navbar-button'>Sign in</button></Link>
        }
        <Link to='/accounts/register' id='right-button'><button className='navbar-button'>Sign Up</button></Link>
    </nav>
    <Outlet />
    </>
  )
}

export default Navbar;