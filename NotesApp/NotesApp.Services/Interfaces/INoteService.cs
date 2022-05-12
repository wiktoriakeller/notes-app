using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto?> GetNoteById(int id);
        Task<IEnumerable<NoteDto>> GetAllNotes();
        Task<IEnumerable<NoteDto>> GetNotesByName(string name);
        Task<IEnumerable<NoteDto>> GetNotesByContent(string content);
        Task<IEnumerable<NoteDto>> GetNotesByTags(IEnumerable<string> tags);
        Task<PublicNoteDto> GetNoteByHashId(string hashId);
        Task<string> UpdateHashId(UpdateNoteHashIdDto dto, int id);
        Task<int> AddNote(CreateNoteDto noteDto);
        Task<NoteDto> UpdateNote(UpdateNoteDto noteDto);
        Task DeleteNote(int id);
    }
}
 