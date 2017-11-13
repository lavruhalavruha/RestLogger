using RestLogger.Infrastructure.Helpers.PasswordHasher.Model;

namespace RestLogger.Infrastructure.Helpers.PasswordHasher
{
    public interface IPasswordWithSaltHasher
    {
        HashWithSaltResult HashPassword(string password);
        bool CheckPassword(string password, string hash, string salt);
    }
}
