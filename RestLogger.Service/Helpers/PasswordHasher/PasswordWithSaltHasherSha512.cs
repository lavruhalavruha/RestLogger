using System.Security.Cryptography;

namespace RestLogger.Service.Helpers.PasswordHasher
{
    public class PasswordWithSaltHasherSha512 : PasswordWithSaltHasherBase
    {
        private HashAlgorithm _hashAlgorithm = null;

        protected override HashAlgorithm HashAlgorithm
        {
            get
            {
                if (_hashAlgorithm == null)
                {
                    _hashAlgorithm = SHA512.Create();
                }

                return _hashAlgorithm;
            }
        }

        protected override int SaltLength => 64;
    }
}
