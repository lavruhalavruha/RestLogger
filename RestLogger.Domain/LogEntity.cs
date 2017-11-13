namespace RestLogger.Domain
{
    public class LogEntity
    {
        public int Id { get; set; }

        public string Logger { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }

        public int ApplicationId { get; set; }
        public virtual ApplicationEntity Application { get; set; }
    }
}
