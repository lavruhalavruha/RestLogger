namespace RestLogger.Infrastructure.Helpers.PasswordHasher.Model
{
    public class HashWithSaltResult
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
