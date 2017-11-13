namespace RestLogger.Infrastructure.Service.Model.LogDtos
{
    public class LogCreateDto
    {
        public int ApplicationId { get; set; }
        public string Logger { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}
