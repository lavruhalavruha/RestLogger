using System.Threading.Tasks;

namespace RestLogger.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }
}
