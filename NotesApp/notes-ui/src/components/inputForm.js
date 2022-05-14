import React from 'react'
import './inputForm.css'

const InputForm = (props) => {
  const {label, name, onChange, errorMessage, isValid, isFocused, onFocus, ...inputProps} = props;

  return (
    <div className='input-form'>
        <label htmlFor={name}>{label}</label>
        <input name={name} {...inputProps} onChange={onChange} onFocus={onFocus} required/>
        <span className={!isValid && isFocused ? 'error-msg' : 'hide'}>{errorMessage}</span>
    </div>
  )
}

export default InputForm