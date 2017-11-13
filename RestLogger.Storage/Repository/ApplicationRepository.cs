using System.Threading.Tasks;

using RestLogger.Domain;
using RestLogger.Infrastructure.Repository;

namespace RestLogger.Storage.Repository
{
    public class ApplicationRepository : RepositoryBase<ApplicationEntity>, IApplicationRepository
    {
        public ApplicationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public ApplicationEntity TryGetByDisplayName(string displayName)
        {
            return base.TryGet(a => a.DisplayName.Trim() == displayName.Trim());
        }

        public async Task<ApplicationEntity> TryGetByDisplayNameAsync(string displayName)
        {
            return await base.TryGetAsync(a => a.DisplayName.Trim() == displayName.Trim());
        }
    }
}
