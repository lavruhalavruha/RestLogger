using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using RestLogger.Infrastructure;

namespace RestLogger.Storage
{
    public class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        private DbContext _dbContext = null;
        private readonly IDatabaseFactory _databaseFactory;

        public RepositoryBase(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void Add(T item)
        {
            DbContext.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            DbContext.Set<T>().Remove(item);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            T item = TryGet(where);

            if (item == null)
            {
                throw new ArgumentOutOfRangeException($"{typeof(T).Name} item satisfying expression '{where}' not found.");
            }

            return item;
        }

        public T TryGet(Expression<Func<T, bool>> where)
        {
            return DbContext.Set<T>().Where(where).SingleOrDefault();
        }

        public async Task<T> TryGetAsync(Expression<Func<T, bool>> where)
        {
            return await DbContext.Set<T>().Where(where).SingleOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return DbContext.Set<T>().ToList();
        }

        public T GetById(object id)
        {
            T item = TryGetById(id);

            if (item == null)
            {
                throw new ArgumentOutOfRangeException($"{typeof(T).Name} item with Id {id} not found.");
            }

            return item;
        }

        public T TryGetById(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public async Task<T> TryGetByIdAsync(object id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbContext.Set<T>().Where(where).ToList();
        }

        public void Update(T item)
        {
            DbContext.Set<T>().Attach(item);
            DbContext.Entry(item).State = EntityState.Modified;
        }

        protected DbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = _databaseFactory.GetDbContext();
                }

                return _dbContext;
            }
        }
    }
}
