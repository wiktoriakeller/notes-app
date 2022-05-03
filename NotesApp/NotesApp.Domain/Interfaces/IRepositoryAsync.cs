using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(int id, string include);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string include);
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> GetAllAsync(string include);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate, string include);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
