using EmitMapper;

using RestLogger.Domain;
using RestLogger.Infrastructure.Service.Model.ApplicationDtos;

namespace RestLogger.Service.Helpers.Mappers.ApplicationMapper
{
    internal static class ApplicationMapper
    {
        public static ApplicationEntity ToApplication(this ApplicationCreateDto applicationCreateDto)
        {
            return ObjectMapperManager
                .DefaultInstance
                .GetMapper<ApplicationCreateDto, ApplicationEntity>()
                .Map(applicationCreateDto);
        }

        public static ApplicationDto ToApplicationDto(this ApplicationEntity application)
        {
            return ObjectMapperManager
                .DefaultInstance
                .GetMapper<ApplicationEntity, ApplicationDto>()
                .Map(application);
        }
    }
}
