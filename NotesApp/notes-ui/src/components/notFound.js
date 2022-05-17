import React from 'react';
import { Link } from 'react-router-dom';
import './styles/notFound.css';

const NotFound = () => (
  <div className='error-page'>
    <h1 className='error-code'>404 - Not Found!</h1>
    <p className='back'><Link to='/'>Go Home</Link></p>
  </div>
);

export default NotFound;