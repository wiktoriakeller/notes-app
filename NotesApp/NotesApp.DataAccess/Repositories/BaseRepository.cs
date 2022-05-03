using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using System.Linq.Expressions;
using NotesApp.Domain.Interfaces;
using System.Reflection;

namespace NotesApp.DataAccess.Repositories
{
    public class BaseRepository<T> : IRepository<T>, IRepositoryAsync<T> where T : BaseEntity
    {
        protected readonly NotesDbContext _dbContext;
        private readonly List<PropertyInfo> properties;

        public BaseRepository(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
            properties = typeof(T).GetProperties().Where(p => p.GetMethod is not null && p.GetMethod.IsVirtual).ToList();
        }

        public virtual Task<T?> GetByIdAsync(int id) => _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public virtual T? GetFirstOrDefault(Expression<Func<T, bool>> predicate) => _dbContext.Set<T>().FirstOrDefault(predicate);

        public virtual Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => _dbContext.Set<T>().FirstOrDefaultAsync<T>(predicate);

        public virtual Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string include)
        {
            if(include != string.Empty && IsVirtualProperty(include))
                return _dbContext.Set<T>().Include(include).FirstOrDefaultAsync(predicate);
        
            return GetFirstOrDefaultAsync(predicate);
        }

        public virtual ICollection<T> GetAllWhere(Expression<Func<T, bool>> predicate) =>
            _dbContext
            .Set<T>()
            .Where(predicate)
            .ToList();

        public virtual async Task<ICollection<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate) => 
            await _dbContext
            .Set<T>()
            .Where(predicate)
            .ToListAsync();

        public virtual async Task<ICollection<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate, string include)
        {
            if (include != string.Empty && IsVirtualProperty(include))
                return await _dbContext.Set<T>().Include(include).Where(predicate).ToListAsync();

            return await GetAllWhereAsync(predicate);
        }

        public ICollection<T> GetAll() => _dbContext.Set<T>().ToList();

        public virtual async Task<ICollection<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

        public virtual async Task<ICollection<T>> GetAllAsync(string include)
        {
            if (include != string.Empty && IsVirtualProperty(include))
                return await _dbContext.Set<T>().Include(include).ToListAsync();

            return await GetAllAsync();
        }

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

        private bool IsVirtualProperty(string include) =>
            properties
            .Any(p => p.Name.ToLower() == include.ToLower().Trim());
    }
}
