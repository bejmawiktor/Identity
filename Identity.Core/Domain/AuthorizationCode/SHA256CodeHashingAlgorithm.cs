using System;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Core.Domain
{
    internal class SHA256CodeHashingAlgorithm : ICodeHashingAlgorithm
    {
        private static readonly int HashLength = 32;

        public byte[] Hash(Code code)
        {
            if(code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            byte[] hashedCodeBytes = null;

            using(SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(code.ToString());

                hashedCodeBytes = sha256Hash.ComputeHash(sourceBytes);
            }

            return hashedCodeBytes;
        }

        public void Validate(byte[] hashedCode)
        {
            if(hashedCode == null)
            {
                throw new ArgumentNullException(nameof(hashedCode));
            }

            if(hashedCode == Array.Empty<byte>())
            {
                throw new ArgumentException("Hashed code can't be empty.");
            }

            if(hashedCode.Length != SHA256CodeHashingAlgorithm.HashLength)
            {
                throw new ArgumentException("Wrong hashed code given.");
            }
        }
    }
}