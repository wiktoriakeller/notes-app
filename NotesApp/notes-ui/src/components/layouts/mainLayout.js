import React from 'react';
import Navbar from '../navbar.js';

const MainLayout = ({children}) => {
  return (
    <>
    <Navbar/>
    <main>{children}</main>
    </>
  )
}

export default MainLayout