import React, { useState, useEffect, useContext } from 'react'
import UserContext from './userContext.js';
import { getAllNotes } from '../notes-api.js';
import { useNavigate } from 'react-router-dom';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const {jwtToken, setJwtToken} = useContext(UserContext);
    const navigate = useNavigate();

    useEffect(() => {
        (
            async() => {
                let response = await getAllNotes(jwtToken, navigate);
        
                if(response.success === true) {
                    setUserNotes(response.data);
                }
            }
        )();
    }, []);
    
    return (
        <>
        <div>Blablal</div>
        <div>UserNotes</div>
        {userNotes.map((note) => {
            return <p key={note.id}>{note}</p>;
        })}
        </>
    )
}

export default UserNotes;