using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using System.Linq.Expressions;

namespace NotesApp.DataAccess.Repositories
{
    public class NoteRepository : BaseRepository<Note>, INoteRepository
    {
        public NoteRepository(NotesDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ICollection<Note>> GetAllNotesWithUsersAndTagsAsync(Expression<Func<Note, bool>> predicate) =>
            await _dbContext
            .Set<Note>()
            .Include(n => n.User)
            .Include(n => n.Tags)
            .Where(predicate)
            .ToListAsync();

        public async Task<ICollection<Note>> GetAllAsync(Expression<Func<Note, bool>> predicate, string include, int pageSize, int pageNumber)
        {
            if (include != string.Empty && IsVirtualProperty(include))
            {
                return await _dbContext
                    .Set<Note>()
                    .Include(include)
                    .Where(predicate)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();
            }

            return await GetAllAsync(predicate);
        }
    }
}
