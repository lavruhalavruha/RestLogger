using System.Threading.Tasks;

using RestLogger.Domain;

namespace RestLogger.Infrastructure.Repository
{
    public interface IApplicationRepository : IRepository<ApplicationEntity>
    {
        ApplicationEntity TryGetByDisplayName(string displayName);
        Task<ApplicationEntity> TryGetByDisplayNameAsync(string displayName);
    }
}
