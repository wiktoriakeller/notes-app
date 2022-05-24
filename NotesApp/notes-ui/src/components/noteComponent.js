import React from 'react';
import './styles/noteComponent.css';

const NoteComponent = (props) => {
  const {onClick, title, content, key} = props;
  return (
    <div className='note-container' onClick={onClick}>
      <div className='note-title'>{title}</div>
      <div className='content-container'>
        <img className='note-image' src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/3a/Cat03.jpg/1024px-Cat03.jpg" alt='User image'/>
        <span className='note-text'>{content}</span>
      </div>
    </div>
  )
}

export default NoteComponent