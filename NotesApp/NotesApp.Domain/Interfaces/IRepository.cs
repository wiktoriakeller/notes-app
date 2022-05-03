using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T? GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        ICollection<T> GetAll();
        ICollection<T> GetAll(Expression<Func<T, bool>> predicate);
    }
}
