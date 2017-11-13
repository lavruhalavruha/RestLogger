using System;
using System.Data.Entity;

using RestLogger.Storage.Context;

namespace RestLogger.Storage
{
    public class DatabaseFactory : IDatabaseFactory, IDisposable
    {
        private DbContext _dbContext = null;

        public DbContext GetDbContext()
        {
            if (_dbContext == null)
            {
                _dbContext = new RestLoggerContext();
            }

            return _dbContext;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}
