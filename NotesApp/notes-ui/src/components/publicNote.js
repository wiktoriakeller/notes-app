import React, { useState, useRef, useEffect } from 'react';
import useNotesApi from '../services/useNotesApi';
import { Link, useNavigate, useParams } from 'react-router-dom';
import './styles/publicNote.css';

const PublicNote = () => {
    const [publicNote, setPublicNote] = useState({});
    const noteLoaded = useRef(false);

    const notesApi = useNotesApi();
    const params = useParams();
    const navigate = useNavigate();
    const linkExpires = useRef('');

    useEffect(() => {(
        async() => {
            let response = await notesApi.getPublicNote(params.id);
            if(response.success === true) {
                setPublicNote(response.data);
                let splitted = response.data.publicLinkValidTill.split('T');
                let time = (splitted[1].split('.'))[0];
                linkExpires.current = `${splitted[0]} ${time}`;
                noteLoaded.current = true;
            }
            else {
                navigate('/not-found');
            }
        })();
    }, []);

    return (
        <>
            {
                noteLoaded.current?
                <div className='public-note-container'>
                    <div className='public-note-title'>{publicNote.noteName}</div>
                    <div className='note-author'>{publicNote.author}</div>
                    <div className='note-content'>{publicNote.content}</div>
                    <div>Link valid till: {linkExpires.current}</div>
                    <div className='tags-block'>
                        {publicNote.tags.map((tag) => (
                            <div className='tag'>
                                {tag.tagName}
                            </div>
                        ))}
                    </div>
                    <p><Link to='/notes'>Go Home</Link></p>
                </div> : <></>
            }
        </>
    )
}

export default PublicNote