using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestLogger.Infrastructure
{
    public interface IRepository<T>
    {
        T GetById(object id);
        T TryGetById(object id);
        Task<T> TryGetByIdAsync(object id);

        T Get(Expression<Func<T, bool>> where);
        T TryGet(Expression<Func<T, bool>> where);
        Task<T> TryGetAsync(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
