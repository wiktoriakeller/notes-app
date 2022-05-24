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
import ShowNote from './showNote';
import './styles/userNotes.css';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const [emptyNotesMsg, setEmptyNotesMsg] = useState('');
    const notesApi = useNotesApi();
    const notesLoaded = useRef(false);

    const [openPostForm, setPostFormOpen] = React.useState(false);

    const [postFormData, setPostFormData] = useState({});
    const [isPostFormValid, setIsValidPostForm] = useState(false);
    const [postFormErrorMsg, setPostFormErrorMsg] = useState([]);
    const [showPostFormErrorMsg, setShowPostFormErrorMsg] = useState(false);

    const [openNoteView, setNoteViewOpen] = useState(false);
    const openedNote = useRef('');

    const handleClickOpenPostForm = () => {
        setPostFormOpen(true);
    };
  
    const handleClosePostForm = () => {
        setPostFormOpen(false);
    };

    const handleClickOpenNoteView = (e, note) => {
        if(openedNote.current === '') {
            setNoteViewOpen(true);
            openedNote.current = note;
        }
    }

    const hancleCloseNoveView = () => {
        setNoteViewOpen(false);
        openedNote.current = '';
    }

    const handlePostFormSubmit = async (e) => {
        e.preventDefault();
        setPostFormErrorMsg([]);
        setShowPostFormErrorMsg(false);
    
        let response = await notesApi.postNote(postFormData);

        if(response.success === true) {
          handleClosePostForm();
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
            <>
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
                    <button className='add-button' onClick={handleClickOpenPostForm}><FontAwesomeIcon icon={faPlus}/></button>
                    <Dialog open={openPostForm} onClose={handleClosePostForm}>
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
                            <button className='form-button cancel' onClick={handleClosePostForm}>Cancel</button>
                        </DialogActions>
                    </Dialog>

                    <Dialog open={openNoteView} onClose={hancleCloseNoveView}>
                        <DialogTitle>{openedNote.current.noteName}</DialogTitle>
                        <DialogContent>
                            <ShowNote note={openedNote.current} />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={hancleCloseNoveView}>Edit</button>
                            <button className='form-button cancel' onClick={hancleCloseNoveView}>Cancel</button>
                        </DialogActions>
                    </Dialog>
                </div>
            </div> 
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
                    return <NoteComponent onClick={(e) => handleClickOpenNoteView(e, note)} title={note.noteName} content={contentSubstrig} key={note.id} />
                })}  
            </div>
            </> : <></>
        }
        </>
    )
}

export default UserNotes;