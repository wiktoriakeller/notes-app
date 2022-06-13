using NotesApp.Domain.Entities;
using NotesApp.Services.Dto;
using System.Linq.Expressions;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto?> GetNoteById(string hashId);
        Task<IEnumerable<NoteDto>> GetAllNotes();
        Task<PagedResult<NoteDto>> GetNotes(NoteQuery query);
        Task<PagedResult<NoteDto>> GetPagedNotes(NoteQuery query);
        Task<PagedResult<NoteDto>> GetPagedNotes(NoteQuery query, Expression<Func<Note, bool>> predicate);
        Task<PublicNoteDto> GetPublicNote(string hashId);
        Task<PublicLinkDto> GeneratePublicLink(CreatePublicLinkDto dto, string hashId);
        Task<string> AddNote(CreateNoteDto noteDto);
        Task<NoteDto> UpdateNote(UpdateNoteDto noteDto, string hashId);
        Task DeleteNote(string hashId);
    }
}
 