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
    }
}
