using System.Data.Entity;

namespace RestLogger.Storage
{
    public interface IDatabaseFactory
    {
        DbContext GetDbContext();
    }
}
