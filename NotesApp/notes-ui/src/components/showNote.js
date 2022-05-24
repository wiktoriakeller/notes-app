import React from 'react';
import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faRotate, faTrash } from '@fortawesome/free-solid-svg-icons';
import './styles/showNote.css';

const ShowNote = (props) => {
    const {note} = props;
    const [linkValidTill, setLinkValidTill] = useState('');

    return (
        <div className='note-display'>
            <div className='note-content'>{note.content}</div>
            <div className='link-label'>Generate public link</div>
            <div className='link-container'>
                <input className='link-field' disabled={true}/>
                <button><FontAwesomeIcon icon={faRotate} /></button>
                <button><FontAwesomeIcon icon={faTrash} /></button>
            </div>
            {
                linkValidTill !== '' ?
                <div>Link valid till: {linkValidTill}</div> : <></>
            }
        </div>
    )
}

export default ShowNote