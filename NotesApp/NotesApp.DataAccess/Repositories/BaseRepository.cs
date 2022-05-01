using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using System.Linq.Expressions;
using NotesApp.Domain.Interfaces;

namespace NotesApp.DataAccess.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly NotesDbContext _dbContext;

        public BaseRepository(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual Task<T?> GetByIdAsync(int id) => _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<ICollection<T>> GetWhereAsync(Expression<Func<T, bool>> predicate) => await _dbContext
            .Set<T>()
            .Where(predicate)
            .OrderBy(e => e.CreatedDate)
            .ToListAsync();

        public virtual async Task<ICollection<T>> GetAllAsync() => await _dbContext.Set<T>().OrderBy(e => e.CreatedDate).ToListAsync();

        public ICollection<T> GetAll() => _dbContext.Set<T>().OrderBy(e => e.CreatedDate).ToList();

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChangesAsync();
        }
    }
}
