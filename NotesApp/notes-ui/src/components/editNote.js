import React, { useState, useEffect } from 'react';
import InputForm from './inputForm';
import './styles/addNote.css';

const EditNote = (props) => {
    const {isFormValid, setIsValidForm, setEditFormData, note, allNotes, 
        errorMsg, setErrorMsg, showErrors, setShowErrors, tagsCopy} = props;

    const [name, setName] = useState(note.noteName);
    const [isNameValid, setIsNameValid] = useState(true);
    const [nameFocus, setNameFocus] = useState(false);
    const nameErrorMsg = "Name should be unique and should contain minimum 3 characters.";

    const [imageLink, setImageLink] = useState(note.imageLink);
    const [isImageLinkValid, setIsImageLinkValid] = useState(true);
    const [imageLinkFocus, setImageLinkFocus] = useState(false);
    const imageLinkErrorMsg = "Link should lead to an image.";

    const [content, setContent] = useState(note.content);
    const [isContentValid, setIsContentValid] = useState(true);
    const [contentFocus, setContentFocus] = useState(false);
    const contentErrorMsg = "Content is required.";

    const [tagInput, setTagInput] = useState('');
    const [tagFocus, setTagFocus] = useState(false);
    const [isTagValid, setIsTagValid] = useState(true);
    const [tags, setTags] = useState(tagsCopy);
    const tagErrorMsg = 'Tags should be unique with maximum length of 10 characters.';
    
    useEffect(() => {
        (async() => {
            let validName = validateName();
            let validContent = validateContent();
            let validLink = await validateImageLink();
            let validTag = validateTag();
            let validForm = validName && validContent && validLink && validTag;
            setIsValidForm(validForm);

            if(validForm) {
                let data = {
                    'hashId': note.hashId,
                    'noteName': name,
                    'content': content,
                    'imageLink': imageLink,
                    'tags': []
                }
    
                for(const tag of tags) {
                    data.tags.push({'tagName': tag});
                }

                setEditFormData(data);
            }
        })();
    }, [name, content, tagInput, imageLink]);

    const validateName = () => {
        let trimmedName = name.trim();
        let isUnique = true;
        for(const otherNote of allNotes) {
            if(otherNote.noteName === trimmedName && otherNote.hashId !== note.hashId) {
                isUnique = false;
                break;
            }
        }
        let isValid = isUnique && trimmedName.length > 2;
        setIsNameValid(isValid);
        return isValid;
    }

    const validateContent = () => {
        let isValid = content !== '';
        setIsContentValid(isValid);
        return isValid;
    }

    const validateTag = () => {
        let trimmedTag = tagInput.trim();
        let unique = trimmedTag === '' || !tags.includes(trimmedTag);
        setIsTagValid(unique);
        return unique;
    }
    
    const validateImageLink = async () => {
        let isValid = false;
        if(imageLink === '' || imageLink === null) {
            isValid = true;
        }
        else if((imageLink.endsWith('.jpg') || imageLink.endsWith('.png') || imageLink.endsWith('.jpeg'))
            && (imageLink.startsWith('https://www') || imageLink.startsWith('http://www'))) {
            isValid = true;
        }
        setIsImageLinkValid(isValid);
        return isValid;
    }

    const onKeyDown = (e) => {
        const trimmed = tagInput.trim();

        if(e.key === "Enter" && trimmed.length > 0 && !tags.includes(trimmed)) {
            e.preventDefault();
            setTags(prev => [...prev, trimmed]);
            setTagInput('');
        }
        else if(e.key === "Backspace" && trimmed.length === 0 && tags.length > 0) {
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
            {errorMsg.map((msg) => {
                return <p className={showErrors ? 'error' : 'hide'}>{msg}</p>;
            })}
            <InputForm
                label='Name'
                name='name'
                type='text'
                value={name}
                autoComplete='off'
                errorMessage={nameErrorMsg}
                isValid={isNameValid}
                isFocused={nameFocus}
                maxLength={40}
                onChange={(e) => setName(e.target.value)}
                onFocus={() => setNameFocus(true)}
            />
            
            <InputForm
                label='Image link'
                name='image'
                type='text'
                value={imageLink}
                autoComplete='off'
                errorMessage={imageLinkErrorMsg}
                isValid={isImageLinkValid}
                isFocused={imageLinkFocus}
                onChange={(e) => setImageLink(e.target.value)}
                onFocus={() => setImageLinkFocus(true)}
            />

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
                <span className={!isContentValid && contentFocus ? 'error-msg' : 'hide-error-msg'}>{contentErrorMsg}</span>
            </div>
            
            <InputForm
                label='Tags'
                name='tags'
                type='text'
                value={tagInput}
                autoComplete='off'
                errorMessage={tagErrorMsg}
                isValid={isTagValid}
                isFocused={tagFocus}
                onChange={(e) => setTagInput(e.target.value)}
                onKeyDown={onKeyDown}
                onFocus={() => setTagFocus(true)}
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

export default EditNote