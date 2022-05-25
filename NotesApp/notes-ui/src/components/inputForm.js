import React from 'react';
import './styles/inputForm.css';

const InputForm = (props) => {
  const {label, name, onChange, errorMessage, isValid, isFocused, onFocus, ...inputProps} = props;

  const getClassName = () => {
    if(!isValid && isFocused)
      return 'input-invalid';
    else if(isValid && isFocused)
      return 'input-valid';
    else 
      return '';
  }

  return (
    <div className='input-form'>
        <label htmlFor={name}>{label}</label>
        <input 
          id={name}
          name={name} 
          {...inputProps} 
          onChange={onChange} 
          onFocus={onFocus} 
          required
          className={getClassName()}
        />
        <span className={!isValid && isFocused ? 'error-msg' : 'hide-error-msg'}>{errorMessage}</span>
    </div>
  )
}

export default InputForm