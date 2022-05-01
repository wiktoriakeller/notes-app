using NotesApp.Domain.Entities;
using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface INotesService
    {
        Task AddNote(CreateNoteDto noteDto);
        Task<IEnumerable<Note>> GetAllNotes();
        Task<IEnumerable<Note>> GetNotesByName(string name);
        Task<IEnumerable<Note>> GetNotesByContent(string content);
        Task<IEnumerable<Note>> GetNotesByTags(IEnumerable<string> tags);
    }
}
