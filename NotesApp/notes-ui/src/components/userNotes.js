import React, { useState, useEffect, useContext } from 'react'
import UserContext from './userContext.js';
import { getAllNotes } from '../notes-api.js';
import { useNavigate } from 'react-router-dom';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const [emptyNotesMsg, setEmptyNotesMsg] = useState('');
    const {jwtToken, setJwtToken} = useContext(UserContext);
    const navigate = useNavigate();

    useEffect(() => {
        (
            async() => {
                let response = await getAllNotes(jwtToken, navigate);
        
                if(response.success === true) {
                    setUserNotes(response.data);
                    if(Object.keys(userNotes).length === 0)
                        setEmptyNotesMsg("It's time to add some notes!");
                }
            }
        )();
    }, []);
    
    return (
        <div>
            <p>{emptyNotesMsg}</p>
            {userNotes.map((note) => {
                return <p key={note.id}>{note}</p>;
            })}  
        </div>
    )
}

export default UserNotes;