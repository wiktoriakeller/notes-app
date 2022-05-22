import React, { useState, useEffect, useRef } from 'react'
import useNotesApi from '../services/useNotesApi';
import './styles/userNotes.css';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const [emptyNotesMsg, setEmptyNotesMsg] = useState('');
    const notesApi = useNotesApi();
    const notesLoaded = useRef(false);

    useEffect(() => {(
        async() => {
            let response = await notesApi.getAllNotes();
            if(response.success === true) {
                setUserNotes(response.data);
                if(Object.keys(userNotes).length === 0)
                    setEmptyNotesMsg("It's time to add some notes!");
                notesLoaded.current = true;
            }
        })();
    }, []);
    
    return (
        <div className='home-container'>
            {
                notesLoaded.current ? 
                <>
                <div className='search' defaultValue={'All'}>
                <select className='search-options'>
                    <option value='all'>All</option>
                    <option value='name'>Name</option>
                    <option value='content'>Content</option>
                    <option value='tags'>Tags</option>
                </select>
                <input className='search-bar' placeholder='Search...'/>
                </div>
                <div className='notes'>
                    <p className='empty-notes-msg'>{emptyNotesMsg}</p>
                    {userNotes.map((note) => {
                        return <p key={note.id}>{note}</p>;
                    })}  
                </div>
                </> :
                <></>
            }
        </div>
    )
}

export default UserNotes;