using NUnit.Framework;

using RestLogger.Infrastructure.Helpers.PasswordHasher;
using RestLogger.Infrastructure.Helpers.PasswordHasher.Model;
using RestLogger.Service.Helpers.PasswordHasher;

namespace RestLogger.Tests.UnitTests
{
    [TestFixture]
    public class PasswordWithSaltHasherSha512Tests
    {
        [TestCase("SomePassword")]
        public void WhenHashingPasswordThenValidPasswordIsMatched(string password)
        {
            // Arrange
            IPasswordWithSaltHasher passwordHasher = new PasswordWithSaltHasherSha512();

            // Act
            HashWithSaltResult hashResult = passwordHasher.HashPassword(password);
            bool isMatched = passwordHasher.CheckPassword(password, hashResult.Hash, hashResult.Salt);

            // Assert
            Assert.AreEqual(true, isMatched);
        }

        [TestCase("SomePassword")]
        public void WhenHashingPasswordThenWrongPasswordIsUnmatched(string password)
        {
            // Arrange
            IPasswordWithSaltHasher passwordHasher = new PasswordWithSaltHasherSha512();
            string wrongPassword = password + "wrong";

            // Act
            HashWithSaltResult hashResult = passwordHasher.HashPassword(password);
            bool isMatched = passwordHasher.CheckPassword(wrongPassword, hashResult.Hash, hashResult.Salt);

            // Assert
            Assert.AreEqual(false, isMatched);
        }
    }
}
