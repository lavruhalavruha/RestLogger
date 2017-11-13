using EmitMapper;

using RestLogger.Infrastructure.Service.Model.ApplicationDtos;
using RestLogger.WebApi.Models.ApplicationModels;

namespace RestLogger.WebApi.Helpers.Mappers.ApplicationMapper
{
    internal static class ApplicationMapper
    {
        public static ApplicationCreateDto ToApplicationCreateDto(this ApplicationCreateModel applicationCreateModel)
        {
            return ObjectMapperManager
                .DefaultInstance
                .GetMapper<ApplicationCreateModel, ApplicationCreateDto>()
                .Map(applicationCreateModel);
        }
    }
}
