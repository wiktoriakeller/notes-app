using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<ICollection<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> GetAll();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
