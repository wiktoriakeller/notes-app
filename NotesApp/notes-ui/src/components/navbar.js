import React from 'react'
import { Link } from 'react-router-dom';
import './styles/navbar.css'

const Navbar = () => {
  return (
    <nav className='navbar'>
        <span className='title'><Link to='/'>NotesHub</Link></span>
        <Link to='/login' id='sign-in-button'><button className='navbar-button'>Sign In</button></Link>
        <Link to='/register' id='sign-up-button'><button className='navbar-button'>Sign Up</button></Link>
    </nav>
  )
}

export default Navbar