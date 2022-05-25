import React from 'react';
import './styles/noteComponent.css';

const NoteComponent = (props) => {
  const {onClick, title, content, imageLink, key} = props;
  return (
    <div className='note-container' onClick={onClick}>
      <div className='note-title'>{title}</div>
      <div className='content-container'>
        {
          (imageLink !== null && imageLink.length > 0) ?
          <img className='note-image' src={imageLink} alt='User image'/> : <></>
        }
        <span className='note-text'>{content}</span>
      </div>
    </div>
  )
}

export default NoteComponent