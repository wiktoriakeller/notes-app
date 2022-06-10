export const nameErrorMsg = "Name should be unique and should contain minimum 3 characters.";
export const imageLinkErrorMsg = "Link should lead to an image.";
export const contentErrorMsg = "Content is required.";
export const tagErrorMsg = "Tags should be unique with maximum length of 10 characters.";

export function validateName(name, noteHashId, notes) {
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

export function validateContent(content) {
    let isValid = content !== '';
    return isValid;
}

export function validateTag(tag, tags) {
    let trimmedTag = tag.trim();
    let unique = trimmedTag === '' || !tags.includes(trimmedTag);
    return unique;
}

export function validateImageLink(imageLink) {
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
