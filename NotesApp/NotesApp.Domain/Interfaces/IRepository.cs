﻿using System.Linq.Expressions;

namespace NotesApp.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<ICollection<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> GetAllAsync();
        ICollection<T> GetAll();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
