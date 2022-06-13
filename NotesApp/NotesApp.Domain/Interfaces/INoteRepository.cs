using NotesApp.Domain.Entities;
using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface INoteRepository : IRepository<Note>, IRepositoryAsync<Note>
    {
        Task<ICollection<Note>> GetAllNotesWithUsersAndTagsAsync(Expression<Func<Note, bool>> predicate);
    }
}
