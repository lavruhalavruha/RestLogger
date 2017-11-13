using RestLogger.Domain;
using RestLogger.Infrastructure.Repository;

namespace RestLogger.Storage.Repository
{
    public class LogRepository : RepositoryBase<LogEntity>, ILogRepository
    {
        public LogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
