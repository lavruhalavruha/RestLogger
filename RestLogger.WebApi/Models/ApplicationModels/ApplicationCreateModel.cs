using System.ComponentModel.DataAnnotations;

namespace RestLogger.WebApi.Models.ApplicationModels
{
    public class ApplicationCreateModel
    {
        [Required]
        [MaxLength(32)]
        public string DisplayName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}