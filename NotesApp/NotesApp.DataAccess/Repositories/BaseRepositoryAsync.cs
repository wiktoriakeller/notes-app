using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using System.Linq.Expressions;
using NotesApp.Domain.Interfaces;

namespace NotesApp.DataAccess.Repositories
{
    public class BaseRepositoryAsync<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly NotesDbContext _dbContext;

        public BaseRepositoryAsync(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual Task<T?> GetById(int id) => _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<ICollection<T>> GetWhere(Expression<Func<T, bool>> predicate) => await _dbContext.Set<T>().Where(predicate).OrderBy(e => e.CreatedDate).ToListAsync();

        public virtual async Task<ICollection<T>> GetAll() => await _dbContext.Set<T>().OrderBy(e => e.CreatedDate).ToListAsync();

        public virtual async Task Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChangesAsync();
        }
    }
}
