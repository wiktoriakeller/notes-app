import React, { useState, useEffect, useRef } from 'react'
import useNotesApi from '../services/useNotesApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch, faPlus } from '@fortawesome/fontawesome-free-solid';
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
                <div className='search' >
                    <select className='search-options' defaultValue={'All'}>
                        <option value='all'>All</option>
                        <option value='name'>Name</option>
                        <option value='content'>Content</option>
                        <option value='tags'>Tags</option>
                    </select>
                    <div className='search-input'>
                        <input className='search-field' placeholder='Search...'/>
                        <button type='submit' className='search-button'><FontAwesomeIcon icon={faSearch}/></button>
                    </div>
                    <button className='add-button'><FontAwesomeIcon icon={faPlus} /></button>
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