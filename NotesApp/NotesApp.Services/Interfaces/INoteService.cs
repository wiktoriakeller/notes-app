using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto?> GetNoteById(string hashId);
        Task<IEnumerable<NoteDto>> GetNotes(NoteQuery query);
        Task<IEnumerable<NoteDto>> GetAllNotes();
        Task<IEnumerable<NoteDto>> GetAllNotes(NoteQuery query);
        Task<IEnumerable<NoteDto>> GetNotesByName(NoteQuery query);
        Task<IEnumerable<NoteDto>> GetNotesByContent(NoteQuery query);
        Task<IEnumerable<NoteDto>> GetNotesByTag(NoteQuery query);
        Task<PublicNoteDto> GetPublicNote(string hashId);
        Task<PublicLinkDto> GeneratePublicLink(CreatePublicLinkDto dto, string hashId);
        Task<string> AddNote(CreateNoteDto noteDto);
        Task<NoteDto> UpdateNote(UpdateNoteDto noteDto, string hashId);
        Task DeleteNote(string hashId);
    }
}
 