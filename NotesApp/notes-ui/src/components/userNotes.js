import React, { useState, useEffect, useRef } from 'react'
import useNotesApi from '../services/useNotesApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch, faPlus, faTrash } from '@fortawesome/fontawesome-free-solid';
import NoteComponent from './noteComponent';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import AddNote from './addNote';
import ShowNote from './showNote';
import EditNote from './editNote';
import './styles/userNotes.css';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const [emptyNotesMsg, setEmptyNotesMsg] = useState('');
    const notesApi = useNotesApi();
    const notesLoaded = useRef(false);

    const [openPostForm, setPostFormOpen] = useState(false);

    const [postFormData, setPostFormData] = useState({});
    const [isPostFormValid, setIsValidPostForm] = useState(false);
    const [postFormErrorMsg, setPostFormErrorMsg] = useState([]);
    const [showPostFormErrorMsg, setShowPostFormErrorMsg] = useState(false);

    const [openNoteView, setNoteViewOpen] = useState(false);
    const openedNote = useRef('');

    const [openEditForm, setEditFormOpen] = useState(false);
    const editedNote = useRef('');

    const [editFormData, setEditFormData] = useState({});
    const [isEditFormValid, setIsEditFormValid] = useState(false);
    const [editFormErrorMsg, setEditFormErrorMsg] = useState([]);
    const [showEditFormErrorMsg, setShowEditFormErrorMsg] = useState(false);

    const handleClickOpenPostForm = () => {
        setPostFormOpen(true);
    };
  
    const handleClosePostForm = () => {
        setPostFormOpen(false);
    };

    const handleClickOpenNoteView = () => {
        setNoteViewOpen(true);
    }

    const handleCloseNoteView = () => {
        setNoteViewOpen(false);
        openedNote.current = '';
    }

    const handleClickOpenEditForm = () => {
        setEditFormOpen(true);
    }

    const handleCloseEditForm = () => {
        setEditFormOpen(false);
        editedNote.current = '';
    }

    const showEditForm = () => {
        editedNote.current = openedNote.current;
        handleCloseNoteView();
        handleClickOpenEditForm();
    }

    const handlePostFormSubmit = async(e) => {
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

    const handleEditFormSubmit = async(e) => {
        e.preventDefault();
        setEditFormErrorMsg([]);
        setShowEditFormErrorMsg(false);
    
        let response = await notesApi.editNote(editFormData);

        if(response.success === true) {
            handleCloseEditForm();
            window.location.reload(false);
        }
        else {
            let errorMessages = [];
            for(const [_, value] of Object.entries(response.errors)) {
                errorMessages.push(value);
            }
            setEditFormErrorMsg(errorMessages);
            setShowEditFormErrorMsg(true);
            setIsEditFormValid(false);
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

    const getTags = () => {
        let tagsArr = [];
        for(const tag of editedNote.current.tags) {
            tagsArr.push(tag.tagName);
        }

        return tagsArr;
    }

    const handleDelete = async (e, hashid) => {
        e.preventDefault();
        let response = await notesApi.deleteNote(editedNote.current.hashId);

        if(response.success === true) {
            handleCloseEditForm();
            window.location.reload(false);
        }
    }
    
    return (
        <>
        {
            notesLoaded.current ? 
            <>
            <div className='home-container'>
                <div className='search' >
                    <select className='search-options' defaultValue={'name'}>
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
                                showErrors={showPostFormErrorMsg}
                                setShowError={setShowPostFormErrorMsg}
                            />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={handlePostFormSubmit} disabled={!isPostFormValid}>Add</button>
                            <button className='form-button cancel' onClick={handleClosePostForm}>Cancel</button>
                        </DialogActions>
                    </Dialog>

                    <Dialog open={openNoteView} onClose={handleCloseNoteView}>
                        <DialogTitle>{openedNote.current.noteName}</DialogTitle>
                        <DialogContent>
                            <ShowNote note={openedNote.current} />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={showEditForm}>Edit</button>
                            <button className='form-button cancel' onClick={handleCloseNoteView}>Cancel</button>
                        </DialogActions>
                    </Dialog>

                    <Dialog open={openEditForm} onClose={handleCloseEditForm}>
                        <DialogTitle>
                            <div className='form-title'>
                                <span className='form-title-text'>Edit note</span>
                                <button className='delete-button' onClick={handleDelete}><FontAwesomeIcon icon={faTrash}/></button>
                            </div>
                        </DialogTitle>
                        <DialogContent>
                            <EditNote 
                                isFormValid={isEditFormValid}
                                setIsValidForm={setIsEditFormValid} 
                                setEditFormData={setEditFormData}
                                note={editedNote.current}
                                allNotes={userNotes}
                                errorMsg={editFormErrorMsg}
                                setErrorMsg={setEditFormErrorMsg}
                                showErrors={showEditFormErrorMsg}
                                setShowErrors={setShowEditFormErrorMsg}
                                tagsCopy={() => getTags()}
                            />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={handleEditFormSubmit} disabled={!isEditFormValid}>Save</button>
                            <button className='form-button cancel' onClick={handleCloseEditForm}>Cancel</button>
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
                    return <NoteComponent onClick={() => {
                        openedNote.current = note;
                        handleClickOpenNoteView();
                    }} 
                    title={note.noteName} content={contentSubstrig} imageLink={note.imageLink} key={note.id} />
                })}  
            </div>
            </> : <></>
        }
        </>
    )
}

export default UserNotes;