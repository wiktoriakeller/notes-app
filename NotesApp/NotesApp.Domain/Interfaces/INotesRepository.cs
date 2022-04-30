using NotesApp.Domain.Entities;
using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface INotesRepository : IRepository<Note>
    {
        Task<Note?> GetByName(string name);
        Task<ICollection<Note>> GetAllWithTags();
        Task<ICollection<Note>> GetAllWithTagsWhere(Expression<Func<Note, bool>> predicate);
    }
}
