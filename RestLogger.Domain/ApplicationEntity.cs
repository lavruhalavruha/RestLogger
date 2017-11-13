using System.Collections.Generic;

namespace RestLogger.Domain
{
    public class ApplicationEntity
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public virtual ICollection<LogEntity> Logs { get; set; }
    }
}
