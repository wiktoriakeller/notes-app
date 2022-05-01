using NotesApp.Domain.Entities;
using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<ICollection<User>> GetAllWithNotesAsync();
        Task<ICollection<User>> GetAllWithNotesWhereAsync(Expression<Func<User, bool>> predicate);
    }
}
