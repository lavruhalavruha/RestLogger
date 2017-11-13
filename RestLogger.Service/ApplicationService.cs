using System;
using System.Threading.Tasks;

using RestLogger.Domain;
using RestLogger.Infrastructure;
using RestLogger.Infrastructure.Helpers.PasswordHasher;
using RestLogger.Infrastructure.Helpers.PasswordHasher.Model;
using RestLogger.Infrastructure.Repository;
using RestLogger.Infrastructure.Service;
using RestLogger.Infrastructure.Service.Model.ApplicationDtos;
using RestLogger.Service.Helpers.Mappers.ApplicationMapper;

namespace RestLogger.Service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordWithSaltHasher _passwordWithSaltHasher;

        public ApplicationService(IApplicationRepository applicationRepository,
            IUnitOfWork unitOfWork,
            IPasswordWithSaltHasher passwordWithSaltHasher
            )
        {
            _applicationRepository = applicationRepository;
            _unitOfWork = unitOfWork;
            _passwordWithSaltHasher = passwordWithSaltHasher;
        }

        public async Task<int> AddApplicationAsync(ApplicationCreateDto applicationCreateDto)
        {
            if (applicationCreateDto == null
                || string.IsNullOrWhiteSpace(applicationCreateDto.DisplayName)
                || string.IsNullOrWhiteSpace(applicationCreateDto.Password))
            {
                throw new ArgumentException("Application name and password must be provided.");
            }

            if (await _applicationRepository.TryGetByDisplayNameAsync(applicationCreateDto.DisplayName) != null)
            {
                throw new ArgumentException($"Application with DisplayName {applicationCreateDto.DisplayName} already exists.");
            }

            ApplicationEntity application = applicationCreateDto.ToApplication();

            HashWithSaltResult hashWithSaltResultDto = _passwordWithSaltHasher.HashPassword(applicationCreateDto.Password);
            application.PasswordHash = hashWithSaltResultDto.Hash;
            application.PasswordSalt = hashWithSaltResultDto.Salt;

            _applicationRepository.Add(application);
            await _unitOfWork.CommitAsync();

            return application.Id;
        }

        public async Task<ApplicationDto> FindApplicationAsync(string displayName, string password)
        {
            ApplicationEntity application = await _applicationRepository.TryGetByDisplayNameAsync(displayName);

            if (application != null && _passwordWithSaltHasher.CheckPassword(password, application.PasswordHash, application.PasswordSalt))
            {
                return application.ToApplicationDto();
            }

            return null;
        }
    }
}
