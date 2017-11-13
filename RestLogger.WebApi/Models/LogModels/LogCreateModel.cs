using System.ComponentModel.DataAnnotations;

namespace RestLogger.WebApi.Models.LogModels
{
    public class LogCreateModel
    {
        [Required]
        public int ApplicationId { get; set; }

        [MaxLength(256)]
        public string Logger { get; set; }

        [MaxLength(256)]
        public string Level { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Message { get; set; }
    }
}