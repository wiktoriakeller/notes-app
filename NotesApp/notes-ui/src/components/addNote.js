import React, { useState, useRef, useEffect } from 'react';
import InputForm from './inputForm';
import './styles/addNote.css';

const AddNote = (props) => {
    const {setIsValidForm, notes} = props;

    const [name, setName] = useState('');
    const [isNameValid, setIsNameValid] = useState(false);
    const [nameFocus, setNameFocus] = useState(false);
    const nameErrorMessage = "Name should be unique with minimal 3 characters lenght.";

    const [content, setContent] = useState('');
    const [isContentValid, setIsContentValid] = useState(false);
    const [contentFocus, setContentFocus] = useState(false);
    const contentErrorMessage = "Content is required.";

    const [tagInput, setTagInput] = useState('');
    const [tagFocus, setTagFocus] = useState(false);
    const [tags, setTags] = useState([]);

    const selectedImage = useRef('');

    const resetImage = () => {
        selectedImage.current.value = '';
    }

    const onTagsChange = (e) => {
        setTagInput(e.target.value);
    };

    useEffect(() => {
        let trimmedName = name.trim();
        let isUnique = true;
        for(const note of notes) {
            if(note.noteName === trimmedName) {
                isUnique = false;
                break;
            }
        }
        setIsNameValid(trimmedName != '' && trimmedName.length > 2 && isUnique);
        setIsValidForm(isNameValid && isContentValid);
    }, [name]);

    useEffect(() => {
        setIsContentValid(content.length > 0);
        setIsValidForm(isNameValid && isContentValid);
    }, [content]);

    const onKeyDown = (e) => {
        const trimmed = tagInput.trim();

        if(e.key === "Enter" && trimmed.length > 0) {
            e.preventDefault();
            setTags(prev => [...prev, trimmed]);
            setTagInput('');
        }
        else if(e.key === "Backspace" && !tagInput.length && tags.length > 0) {
            e.preventDefault();
            const tagsCopy = [...tags];
            const poppedTag = tagsCopy.pop();
            setTags(tagsCopy);
            setTagInput(poppedTag);
        }
    }

    const deleteTag = (index) => {
        setTags(prev => prev.filter((tag, i) => i !== index));
    }

    return (
        <form className='form-container'>
            <InputForm
                label='Name'
                name='name'
                type='text'
                value={name}
                autocomplete='off'
                errorMessage={nameErrorMessage}
                isValid={isNameValid}
                isFocused={nameFocus}
                maxLength={40}
                onChange={(e) => setName(e.target.value)}
                onFocus={() => setNameFocus(true)}
            />
            
            <div className='file-field'>
                <input className='upload-file-button' type="file" accept="image/png, image/jpeg" ref={selectedImage}/>
                <button className='form-button reset-button' onClick={resetImage}>Reset</button>
            </div>

            <div className='content-field'>
                <label className='input-form-label' htmlFor='content'>Content</label>
                <textarea 
                    id='content' 
                    maxLength={1000} 
                    rows={12} 
                    required
                    value={content}
                    onChange={(e) => setContent(e.target.value)}
                    onFocus={() => setContentFocus(true)}
                    className={isContentValid && contentFocus ? 'textarea-valid' : contentFocus ? 'textarea-invalid' : 'textarea-normal'}
                    >
                </textarea>
                <span className={!isContentValid && contentFocus ? 'error-msg' : 'hide-error-msg'}>{contentErrorMessage}</span>
            </div>
            
            <InputForm
                label='Tags'
                name='tags'
                type='text'
                value={tagInput}
                autocomplete='off'
                errorMessage={''}
                isValid={true}
                isFocused={tagFocus}
                onChange={onTagsChange}
                onKeyDown={onKeyDown}
                onFocus={() => setTagFocus(false)}
                maxLength={10}
            />
            <div className='tags-block'>
                {tags.map((tag, index) => (
                    <div className='tag'>
                        <button onClick={() => deleteTag(index)}>
                            <div className='close'>
                                x
                            </div>
                        </button>
                        {tag}
                    </div>
                ))}
            </div>
        </form>
    )
}

export default AddNote