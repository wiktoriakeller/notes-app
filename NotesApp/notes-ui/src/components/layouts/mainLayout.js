import React from 'react';
import Navbar from '../navbar.js';

const MainLayout = ({children}) => {
  return (
    <>
    <div>
        <Navbar/>
    </div>
    <main>{children}</main>
    </>
  )
}

export default MainLayout