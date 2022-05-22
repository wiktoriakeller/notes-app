import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import useNotesApi from '../services/useNotesApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFilePen } from '@fortawesome/free-solid-svg-icons';
import './styles/navbar.css'

const Navbar = () => {
  const notesApi = useNotesApi();

  return (
    <>
    <nav className='navbar'>
        <Link to='/notes'><FontAwesomeIcon className='main-icon' icon={faFilePen}/></Link>
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