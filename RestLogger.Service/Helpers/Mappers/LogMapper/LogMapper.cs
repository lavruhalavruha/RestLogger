using EmitMapper;

using RestLogger.Domain;
using RestLogger.Infrastructure.Service.Model.LogDtos;

namespace RestLogger.Service.Helpers.Mappers.LogMapper
{
    internal static class LogMapper
    {
        public static LogEntity ToLog(this LogCreateDto logCreateDto)
        {
            return ObjectMapperManager
                .DefaultInstance
                .GetMapper<LogCreateDto, LogEntity>()
                .Map(logCreateDto);
        }
    }
}
