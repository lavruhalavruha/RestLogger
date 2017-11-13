using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;

using RestLogger.Infrastructure;

namespace RestLogger.Storage
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _dbContext = databaseFactory.GetDbContext();
        }

        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                HandleDbEntityValidationException(ex);

                throw;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                HandleDbEntityValidationException(ex);

                throw;
            }
        }

        private void HandleDbEntityValidationException(DbEntityValidationException ex)
        {
            foreach (var validationErrors in ex.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Debug.WriteLine("Property: {0} Error: {1}",
                                            validationError.PropertyName,
                                            validationError.ErrorMessage);
                }
            }
        }
    }
}
