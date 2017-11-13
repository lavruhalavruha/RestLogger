using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using RestLogger.Infrastructure.Helpers.PasswordHasher;
using RestLogger.Infrastructure.Helpers.PasswordHasher.Model;

namespace RestLogger.Service.Helpers.PasswordHasher
{
    public abstract class PasswordWithSaltHasherBase : IPasswordWithSaltHasher
    {
        protected abstract HashAlgorithm HashAlgorithm { get; }
        protected abstract int SaltLength { get; }

        public bool CheckPassword(string password, string hash, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            HashWithSaltResult result = HashPassword(passwordBytes, saltBytes);

            return result.Hash == hash;
        }

        public HashWithSaltResult HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = GenerateSalt();

            return HashPassword(passwordBytes, saltBytes);
        }

        protected byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltLength];
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(salt);

            return salt;
        }

        protected HashWithSaltResult HashPassword(byte[] passwordBytes, byte[] saltBytes)
        {
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordBytes);
            passwordWithSaltBytes.AddRange(saltBytes);

            byte[] hashBytes = HashAlgorithm.ComputeHash(passwordWithSaltBytes.ToArray());

            return new HashWithSaltResult()
            {
                Hash = Convert.ToBase64String(hashBytes),
                Salt = Convert.ToBase64String(saltBytes)
            };
        }
    }
}
