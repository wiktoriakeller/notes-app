using NotesApp.Domain.Entities;
using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>, IRepositoryAsync<User>
    {
        Task<ICollection<User>> GetAllWithNotesAndTagsAsync();
        Task<ICollection<User>> GetAllWithNotesAndTagsAsync(Expression<Func<User, bool>> predicate);
    }
}
