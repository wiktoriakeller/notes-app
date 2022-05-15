import React from 'react';
import { Link } from 'react-router-dom';
import './notFound.css';

const NotFound = () => (
  <div className='error-page'>
    <h1 className='not-found'>404 - Not Found!</h1>
    <p className='back'><Link to='/'>Go Home</Link></p>
  </div>
);

export default NotFound;