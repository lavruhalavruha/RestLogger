using System.Threading.Tasks;

using RestLogger.Infrastructure.Service.Model.LogDtos;

namespace RestLogger.Infrastructure.Service
{
    public interface ILogService
    {
        Task<int> AddLogAsync(LogCreateDto logCreateDto, string applicationDisplayName);
    }
}
