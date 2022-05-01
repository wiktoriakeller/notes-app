using NotesApp.Domain.Entities;
using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface INotesRepository : IRepository<Note>
    {
        Task<Note?> GetByNameAsync(string name);
        Task<ICollection<Note>> GetAllWithTagsAsync();
        Task<ICollection<Note>> GetAllWithTagsWhereAsync(Expression<Func<Note, bool>> predicate);
    }
}
