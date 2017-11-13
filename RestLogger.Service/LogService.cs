using System;
using System.Threading.Tasks;

using RestLogger.Domain;
using RestLogger.Infrastructure;
using RestLogger.Infrastructure.Repository;
using RestLogger.Infrastructure.Service;
using RestLogger.Infrastructure.Service.Model.LogDtos;
using RestLogger.Service.Helpers.Mappers.LogMapper;

namespace RestLogger.Service
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogService(ILogRepository logRepository,
            IApplicationRepository applicationRepository,
            IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _applicationRepository = applicationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddLogAsync(LogCreateDto logCreateDto, string applicationDisplayName)
        {
            ApplicationEntity application = await _applicationRepository.TryGetByIdAsync(logCreateDto.ApplicationId);

            if (application == null)
            {
                throw new ArgumentException($"Application with id {logCreateDto.ApplicationId} not found.");
            }

            if (application.DisplayName != applicationDisplayName)
            {
                throw new ArgumentException("Application display name mismatch.");
            }

            LogEntity log = logCreateDto.ToLog();

            _logRepository.Add(log);
            await _unitOfWork.CommitAsync();

            return log.Id;
        }
    }
}
