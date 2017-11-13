using System.Threading.Tasks;

using RestLogger.Infrastructure.Service.Model.ApplicationDtos;

namespace RestLogger.Infrastructure.Service
{
    public interface IApplicationService
    {
        Task<int> AddApplicationAsync(ApplicationCreateDto applicationCreateDto);
        Task<ApplicationDto> FindApplicationAsync(string displayName, string password);
    }
}
