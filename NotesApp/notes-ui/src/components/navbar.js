import React from 'react'
import './navbar.css'
import { Link } from 'react-router-dom';

const Navbar = () => {
  return (
    <nav className='navbar'>
        <span className='title'>NotesHub</span>
        <button className='navbar-button' id='sign-in-button'><Link to='/login'>Sign In</Link></button>
        <button className='navbar-button' id='sign-up-button'><Link to='/register'>Sign Up</Link></button>
    </nav>
  )
}

export default Navbar