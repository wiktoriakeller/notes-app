function validateName(name, noteHashId, notes) {
    let trimmedName = name.trim();
    let isUnique = true;
    for(const note of notes) {
        if(note.noteName === trimmedName && note.hashId !== noteHashId) {
            isUnique = false;
            break;
        }
    }
    let isValid = isUnique && trimmedName.length > 2;
    return isValid;
}

function validateContent(content) {
    let isValid = content !== '';
    return isValid;
}

function validateTag(tag, tags) {
    let trimmedTag = tag.trim();
    let unique = trimmedTag === '' || !tags.includes(trimmedTag);
    return unique;
}

function validateImageLink(imageLink) {
    let isValid = false;
    if(imageLink === '' || imageLink === null) {
        isValid = true;
    }
    else if((imageLink.endsWith('.jpg') || imageLink.endsWith('.png') || imageLink.endsWith('.jpeg'))
        && (imageLink.startsWith('https://') || imageLink.startsWith('http://'))) {
        isValid = true;
    }
    return isValid;
}

export {validateName, validateContent, validateTag, validateImageLink};