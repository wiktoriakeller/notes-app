import React from 'react'
import { Link, Outlet } from 'react-router-dom';
import './styles/navbar.css'

const Navbar = () => {
  return (
    <>
    <nav className='navbar'>
        <span className='title'><Link to='/notes'>NotesHub</Link></span>
        <Link to='/accounts/login' id='sign-in-button'><button className='navbar-button'>Sign In</button></Link>
        <Link to='/accounts/register' id='sign-up-button'><button className='navbar-button'>Sign Up</button></Link>
    </nav>
    <Outlet />
    </>
  )
}

export default Navbar;