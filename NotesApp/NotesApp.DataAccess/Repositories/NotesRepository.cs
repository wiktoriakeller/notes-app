using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace NotesApp.DataAccess.Repositories
{
    public class NotesRepository : BaseRepositoryAsync<Note>, INotesRepository
    {
        public NotesRepository(NotesDbContext dbContext) : base(dbContext)
        {

        }

        public Task<Note?> GetByName(string name) => _dbContext.Notes.FirstOrDefaultAsync(n => n.NoteName == name);

        public async Task<ICollection<Note>> GetAllWithTags() => await _dbContext.Notes.Include(n => n.Tags).ToListAsync();

        public async Task<ICollection<Note>> GetAllWithTagsWhere(Expression<Func<Note, bool>> predicate) => await _dbContext.Notes.Include(n => n.Tags).Where(predicate).ToListAsync();
    }
}
