using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using System.Linq.Expressions;

namespace NotesApp.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(NotesDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<User>> GetAllWithNotesAndTagsAsync() => 
            await _dbContext.Users
            .Include(u => u.Notes)
            .ThenInclude(n => n.Tags)
            .OrderBy(u => u.CreatedDate)
            .ToListAsync();

        public async Task<ICollection<User>> GetAllWithNotesAndTagsAsync(Expression<Func<User, bool>> predicate) => 
            await _dbContext.Users
            .Include(u => u.Notes)
            .ThenInclude(n => n.Tags)
            .Where(predicate)
            .OrderBy(u => u.CreatedDate)
            .ToListAsync();
    }
}
