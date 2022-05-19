import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom';
import useNotesApi from '../services/useNotesApi';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const [emptyNotesMsg, setEmptyNotesMsg] = useState('');
    const navigate = useNavigate();
    const notesApi = useNotesApi();

    useEffect(() => {
        (
            async() => {
                let response = await notesApi.getAllNotes();
        
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