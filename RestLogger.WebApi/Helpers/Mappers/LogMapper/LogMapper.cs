using EmitMapper;

using RestLogger.Infrastructure.Service.Model.LogDtos;
using RestLogger.WebApi.Models.LogModels;

namespace RestLogger.WebApi.Helpers.Mappers.LogMapper
{
    internal static class LogMapper
    {
        public static LogCreateDto ToLogCreateDto(this LogCreateModel logCreateModel)
        {
            return ObjectMapperManager
                .DefaultInstance
                .GetMapper<LogCreateModel, LogCreateDto>()
                .Map(logCreateModel);
        }
    }
}
