import React, { useState, useEffect, useRef } from 'react'
import useNotesApi from '../services/useNotesApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch, faPlus } from '@fortawesome/fontawesome-free-solid';
import NoteComponent from './noteComponent';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import AddNote from './addNote';
import './styles/userNotes.css';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const [emptyNotesMsg, setEmptyNotesMsg] = useState('');
    const notesApi = useNotesApi();
    const notesLoaded = useRef(false);

    const [open, setOpen] = React.useState(false);

    const [postFormData, setPostFormData] = useState({});
    const [isPostFormValid, setIsValidPostForm] = useState(false);
    const [postFormErrorMsg, setPostFormErrorMsg] = useState([]);
    const [showPostFormErrorMsg, setShowPostFormErrorMsg] = useState(false);

    const handleClickOpen = () => {
      setOpen(true);
    };
  
    const handleClose = () => {
      setOpen(false);
    };

    const handlePostFormSubmit = async (e) => {
        e.preventDefault();
        setPostFormErrorMsg([]);
        setShowPostFormErrorMsg(false);
    
        let response = await notesApi.postNote(postFormData);

        if(response.success === true) {
          handleClose();
          window.location.reload(false);
        }
        else {
          let errorMessages = [];
          for(const [_, value] of Object.entries(response.errors)) {
            errorMessages.push(value);
          }
          setPostFormErrorMsg(errorMessages);
          setShowPostFormErrorMsg(true);
          setIsValidPostForm(false);
        }
    }

    useEffect(() => {(
        async() => {
            let response = await notesApi.getAllNotes();
            if(response.success === true) {
                setUserNotes(response.data);
                if(Object.keys(response.data).length === 0)
                    setEmptyNotesMsg("It's time to add some notes!");
                notesLoaded.current = true;
            }
        })();
    }, []);
    
    return (
        <>
        {
            notesLoaded.current ? 
            <div className='home-container'>
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
                    <button className='add-button' onClick={handleClickOpen}><FontAwesomeIcon icon={faPlus}/></button>
                    <Dialog open={open} onClose={handleClose}>
                        <DialogTitle>Add new note</DialogTitle>
                        <DialogContent>
                            <AddNote 
                            isFormValid={isPostFormValid}
                            setIsValidForm={setIsValidPostForm} 
                            setPostFormData={setPostFormData}
                            notes={userNotes}
                            errorMsg={postFormErrorMsg}
                            setErrorMsg={setPostFormErrorMsg}
                            showErrorMsg={showPostFormErrorMsg}
                            setShowErrorMsg={setShowPostFormErrorMsg}
                            />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={handlePostFormSubmit} disabled={!isPostFormValid}>Add</button>
                            <button className='form-button cancel' onClick={handleClose}>Cancel</button>
                        </DialogActions>
                    </Dialog>
                </div>
            </div> : <></>
        }
        {
            notesLoaded.current ?
            <>
            <div className='notes'>
                {
                    emptyNotesMsg.length > 0 ?
                    <p className='empty-notes-msg'>{emptyNotesMsg}</p> :
                    <></>
                }
                {userNotes.map((note) => {
                    const cutOff = 100;
                    let contentSubstrig = note.content.substring(0, cutOff);
                    if(note.content.length > cutOff)
                        contentSubstrig += '...';
                    return <NoteComponent title={note.noteName} content={contentSubstrig} key={note.id} />
                })}  
            </div>
            </> : <></>
        }
        </>
    )
}

export default UserNotes;