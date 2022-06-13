import React from 'react'
import InputForm from './inputForm';
import * as validation from '../services/noteValidation';

const Tags = (props) => {
    const {tags, setTags, tagInput, setTagInput, isTagValid, tagFocus, allowDeleting, onChange, onFocus} = props;

    const onKeyDown = (e) => {
        const trimmed = tagInput.trim();

        if(e.key === "Enter" && trimmed.length > 0 && !tags.includes(trimmed) && isTagValid) {
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

    const deleteTag = (e, index) => {
        e.preventDefault();
        if(e.type === "mousedown") {
            setTags(prev => prev.filter((tag, i) => i !== index));
        }
    }

    return (
        <>
            <InputForm
                label='Tags'
                name='tags'
                type='text'
                value={tagInput}
                autoComplete='off'
                errorMessage={validation.tagErrorMsg}
                isValid={isTagValid}
                isFocused={tagFocus}
                onChange={onChange}
                onKeyDown={onKeyDown}
                onFocus={onFocus}
                maxLength={10}
            />

            <div className='tags-block'>
                {tags.map((tag, index) => (
                    allowDeleting === true ?
                    <div className='tag'>
                        <button onClick={(e) => deleteTag(e, index)}>
                            <div className='close'>
                                x
                            </div>
                        </button>
                        {tag}
                    </div> : <></>
                ))}
            </div>
        </>
    )
}

export default Tags