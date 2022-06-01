using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto?> GetNoteById(string hashId);
        Task<IEnumerable<NoteDto>> GetNotes(string? type, string? value);
        Task<IEnumerable<NoteDto>> GetAllNotes();
        Task<IEnumerable<NoteDto>> GetAllNotes(string value);
        Task<IEnumerable<NoteDto>> GetNotesByName(string name);
        Task<IEnumerable<NoteDto>> GetNotesByContent(string content);
        Task<IEnumerable<NoteDto>> GetNotesByTag(string tags);
        Task<PublicNoteDto> GetPublicNote(string hashId);
        Task<PublicLinkDto> GeneratePublicLink(CreatePublicLinkDto dto, string hashId);
        Task<string> AddNote(CreateNoteDto noteDto);
        Task<NoteDto> UpdateNote(UpdateNoteDto noteDto, string hashId);
        Task DeleteNote(string hashId);
    }
}
 