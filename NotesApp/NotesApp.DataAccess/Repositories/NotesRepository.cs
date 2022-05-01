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

        public override Task<Note?> GetById(int id) => _dbContext.Notes
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.Id == id);

        public Task<Note?> GetByName(string name) => _dbContext.Notes
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NoteName == name);

        public async Task<ICollection<Note>> GetAllWithTags() => await _dbContext.Notes
            .Include(n => n.Tags)
            .OrderBy(n => n.CreatedDate)
            .ToListAsync();

        public async Task<ICollection<Note>> GetAllWithTagsWhere(Expression<Func<Note, bool>> predicate) => await _dbContext.Notes
            .Include(n => n.Tags)
            .Where(predicate)
            .OrderBy(n => n.CreatedDate)
            .ToListAsync();
    }
}
