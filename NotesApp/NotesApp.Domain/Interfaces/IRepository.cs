using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        ICollection<T> GetAllWhere(Expression<Func<T, bool>> predicate);
        ICollection<T> GetAll();
    }
}
