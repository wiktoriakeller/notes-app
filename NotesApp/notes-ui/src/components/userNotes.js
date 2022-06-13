import React, { useState, useEffect, useRef } from 'react'
import useNotesApi from '../services/useNotesApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch, faPlus, faTrash } from '@fortawesome/fontawesome-free-solid';
import NoteComponent from './noteComponent';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { DialogContentText } from '@mui/material';
import AddNote from './addNote';
import NoteView from './noteView';
import EditNote from './editNote';
import ReactPaginate from 'react-paginate';
import './styles/userNotes.css';
import './styles/pagination.css';

const UserNotes = () => {
    const [userNotes, setUserNotes] = useState([]);
    const notesApi = useNotesApi();
    const notesLoaded = useRef(false);

    const [isAddFormOpened, setIsAddFormOpened] = useState(false);
    const [addFormData, setAddFormData] = useState({});
    const [isAddFormValid, setIsAddFormValid] = useState(false);
    const [addFormErrorMsgs, setAddFormErrorMsgs] = useState([]);

    const [isNoteViewOpened, setIsNoteViewOpened] = useState(false);
    const openedNote = useRef('');

    const [isEditFormOpened, setIsEditFormOpened] = useState(false);
    const [editFormData, setEditFormData] = useState({});
    const [isEditFormValid, setIsEditFormValid] = useState(false);
    const [editFormErrorMsgs, setEditFormErrorMsgs] = useState([]);
    const editedNote = useRef('');

    const [selectedSearch, setSelectedSearch] = useState('All');
    const [selectedValue, setSelectedValue] = useState('');

    const [isQuestionPopupOpened, setQuestionPopupOpened] = useState(false);

    const pageSize = 20;
    const [currentPageNumber, setCurrentPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {(
        async() => {
            await searchNotes();
        })();
    }, [currentPageNumber]);

    const searchNotes = async() => {
        let response = await notesApi.getNotes(selectedSearch, selectedValue, pageSize, currentPageNumber);

        if(response.success === true) {
            setUserNotes(response.data.items);
            setTotalPages(response.data.totalPages);
            notesLoaded.current = true;
        }
    }

    const openAddForm = (isOpen) => {
        setIsAddFormOpened(isOpen);
    };

    const openNoteView = (isOpen) => {
        setIsNoteViewOpened(isOpen);
        if (isOpen === false) {
            openedNote.current = '';
        }
    }

    const openEditForm = (isOpen) => {
        setIsEditFormOpened(isOpen);
        if(isOpen === true) {
            editedNote.current = openedNote.current;
            openNoteView(false);
        }
        else {
            editedNote.current = '';
        }
    }

    const openQuestionPopup = (isOpen) => {
        setQuestionPopupOpened(isOpen);
    }

    const handleAgreeQuestionPopup = async(e) => {
        await handleNoteDelete(e)
    }

    const submitNote = async(e) => {
        e.preventDefault();
        setAddFormErrorMsgs([]);
    
        let response = await notesApi.postNote(addFormData);

        if(response.success === true) {
            openAddForm(false);
            window.location.reload(false);
        }
        else {
            let errorMessages = [];
            for(const [_, value] of Object.entries(response.errors)) {
                errorMessages.push(value);
            }
            setAddFormErrorMsgs(errorMessages);
            setIsAddFormValid(false);
        }
    }

    const submitEditNote = async(e) => {
        e.preventDefault();
        setEditFormErrorMsgs([]);
    
        let response = await notesApi.editNote(editFormData, editedNote.current.hashId);

        if(response.success === true) {
            openEditForm(false);
            window.location.reload(false);
        }
        else {
            let errorMessages = [];
            for(const [_, value] of Object.entries(response.errors)) {
                errorMessages.push(value);
            }
            
            setEditFormErrorMsgs(errorMessages);
            setIsEditFormValid(false);
        }
    }

    const getTags = () => {
        let tagsArr = [];
        for(const tag of editedNote.current.tags) {
            tagsArr.push(tag.tagName);
        }

        return tagsArr;
    }

    const handleNoteDelete = async(e) => {
        e.preventDefault();
        let response = await notesApi.deleteNote(editedNote.current.hashId);

        if(response.success === true) {
            openEditForm(false);
            window.location.reload(false);
        }
    }

    const handlePageClick = ({ selected: selectedPage}) => {
        setCurrentPageNumber(selectedPage + 1);
    }
    
    return (
        <>
        {
            notesLoaded.current ? 
            <>
            <div className='home-container'>
                <div className='search' >
                    <select className='search-options' defaultValue={'all'} onChange={(e) => setSelectedSearch(e.target.value)}>
                        <option value='all'>All</option>
                        <option value='name'>Name</option>
                        <option value='content'>Content</option>
                        <option value='tags'>Tags</option>
                    </select>
                    <div className='search-input'>
                        <input className='search-field' placeholder='Search...' onChange={(e) => setSelectedValue(e.target.value)}/>
                        <button onClick={searchNotes} type='submit' className='search-button'><FontAwesomeIcon icon={faSearch}/></button>
                    </div>
                    <button className='add-button' onClick={() => openAddForm(true)}><FontAwesomeIcon icon={faPlus}/></button>

                    <Dialog open={isAddFormOpened} onClose={() => openAddForm(false)}>
                        <DialogTitle>Add new note</DialogTitle>
                        <DialogContent>
                            <AddNote 
                                setIsValidForm={setIsAddFormValid} 
                                setPostFormData={setAddFormData}
                                notes={userNotes}
                                errorMsg={addFormErrorMsgs}
                                setErrorMsg={setAddFormErrorMsgs}
                            />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={submitNote} disabled={!isAddFormValid}>Add</button>
                            <button className='form-button cancel' onClick={() => openAddForm(false)}>Cancel</button>
                        </DialogActions>
                    </Dialog>

                    <Dialog open={isNoteViewOpened} onClose={() => openNoteView(false)}>
                        <DialogTitle>{openedNote.current.noteName}</DialogTitle>
                        <DialogContent>
                            <NoteView noteRef={openedNote} note={openedNote.current} />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={() => openEditForm(true)}>Edit</button>
                            <button className='form-button cancel' onClick={() => openNoteView(false)}>Cancel</button>
                        </DialogActions>
                    </Dialog>

                    <Dialog open={isEditFormOpened} onClose={() => openEditForm(false)}>
                        <DialogTitle>
                            <div className='form-title'>
                                <span className='form-title-text'>Edit note</span>
                                <button className='delete-button' onClick={() => openQuestionPopup(true)}><FontAwesomeIcon icon={faTrash}/></button>
                            </div>
                        </DialogTitle>
                        <DialogContent>
                            <EditNote 
                                setIsValidForm={setIsEditFormValid} 
                                setEditFormData={setEditFormData}
                                note={editedNote.current}
                                allNotes={userNotes}
                                errorMsg={editFormErrorMsgs}
                                setErrorMsg={setEditFormErrorMsgs}
                                tagsCopy={() => getTags()}
                            />
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button' onClick={submitEditNote} disabled={!isEditFormValid}>Save</button>
                            <button className='form-button cancel' onClick={() => openEditForm(false)}>Cancel</button>
                        </DialogActions>
                    </Dialog>

                    <Dialog
                        open={isQuestionPopupOpened}
                        onClose={() => openQuestionPopup(false)}
                        aria-labelledby="alert-dialog-title"
                        aria-describedby="alert-dialog-description">
                        <DialogTitle id="alert-dialog-title">
                            {"Delete note"}
                        </DialogTitle>
                        <DialogContent>
                            <DialogContentText id="alert-dialog-description">
                                Are you sure tha you want to delete this note?
                            </DialogContentText>
                        </DialogContent>
                        <DialogActions>
                            <button className='form-button cancel' onClick={handleAgreeQuestionPopup} autoFocus>Yes</button>
                            <button className='form-button' onClick={() => openQuestionPopup(false)}>No</button>
                        </DialogActions>
                    </Dialog>
                </div>
            </div> 
            <div className='notes-wrapper'>
                <div className='notes'>
                    {
                        userNotes.length === 0 ?
                        <p className='empty-notes-msg'>{"It's empty here!"}</p> : <></>
                    }
                    {userNotes.map((note) => {
                        const cutOff = 100;
                        let contentSubstrig = note.content.substring(0, cutOff);
                        if(note.content.length > cutOff)
                            contentSubstrig += '...';

                        return <NoteComponent onClick={() => {
                            openedNote.current = note;
                            openNoteView(true);
                        }} 
                        title={note.noteName} content={contentSubstrig} imageLink={note.imageLink} key={note.id} />
                    })}  
                </div>
                <ReactPaginate
                    breakLabel={"..."}
                    previousLabel={"Previous"}
                    nextLabel={"Next"}
                    pageCount={totalPages}
                    onPageChange={handlePageClick}
                    containerClassName={"pagination"}
                    pageLinkClassName={"pagination-link-active"}
                    pageClassName={"pagination-link-active"}
                    previousClassName={"pagination-link-active"}
                    nextClassName={"pagination-link-active"}
                    previousLinkClassName={"pagination-link-active"}
                    nextLinkClassName={"pagination-link"}
                    disabledClassName={"pagination-link-disabled"}
                    activeClassName={"active-page"}
                    activeLinkClassName={"pagination-link-active"}
                />
            </div>
            </> : <></>
        }
        </>
    )
}

export default UserNotes;