using System;
using System.Security.Cryptography;

namespace Identity.Core.Domain
{
    internal sealed class Pbkdf2HashingAlgorithm : IPasswordHashingAlgorithm
    {
        public int Pbkdf2IterCount => 50000;
        public int Pbkdf2SubkeyLength => 256 / 8;
        public int SaltLength => 128 / 8;
        public HashAlgorithmName HashAlgorithmName => HashAlgorithmName.SHA256;
        public int HashedPasswordLength => this.SaltLength + this.Pbkdf2SubkeyLength;

        public byte[] Hash(Password password)
        {
            if(password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] salt;
            byte[] pbkdf2Subkey;

            using(Rfc2898DeriveBytes rfc2898DeriveBytes = new(password.ToString(), this.SaltLength, this.Pbkdf2IterCount, this.HashAlgorithmName))
            {
                salt = rfc2898DeriveBytes.Salt;
                pbkdf2Subkey = rfc2898DeriveBytes.GetBytes(this.Pbkdf2SubkeyLength);
            }

            return this.AssemblyHashedPassword(salt, pbkdf2Subkey);
        }

        private byte[] AssemblyHashedPassword(byte[] salt, byte[] pbkdf2Subkey)
        {
            byte[] hashedPassword = new byte[this.HashedPasswordLength];

            Buffer.BlockCopy(salt, 0, hashedPassword, 0, this.SaltLength);
            Buffer.BlockCopy(pbkdf2Subkey, 0, hashedPassword, this.SaltLength, this.Pbkdf2SubkeyLength);

            return hashedPassword;
        }

        public void Validate(byte[] hashedPassword)
        {
            if(hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if(this.HashedPasswordLength != hashedPassword.Length)
            {
                throw new ArgumentException("Incorrect hashed password given.");
            }
        }

        public PasswordVerificationResult Verify(byte[] hashedPassword, Password verifiedPassword)
        {
            if(verifiedPassword == null)
            {
                throw new ArgumentNullException(nameof(verifiedPassword));
            }

            this.Validate(hashedPassword);

            byte[] salt = this.ExtractSalt(hashedPassword);
            byte[] pbkdf2Subkey = this.ExtractPbkdf2Subkey(hashedPassword);
            byte[] verifiedPasswordBytes;

            using(Rfc2898DeriveBytes rfc2898DeriveBytes = new(verifiedPassword.ToString(), salt, this.Pbkdf2IterCount, this.HashAlgorithmName))
            {
                verifiedPasswordBytes = rfc2898DeriveBytes.GetBytes(this.Pbkdf2SubkeyLength);
            }

            if(CryptographicOperations.FixedTimeEquals(pbkdf2Subkey, verifiedPasswordBytes))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }

        private byte[] ExtractSalt(byte[] hashedPassword)
        {
            byte[] salt = new byte[this.SaltLength];

            Buffer.BlockCopy(hashedPassword, 0, salt, 0, this.SaltLength);

            return salt;
        }

        private byte[] ExtractPbkdf2Subkey(byte[] hashedPassword)
        {
            byte[] pbkdf2Subkey = new byte[this.Pbkdf2SubkeyLength];

            Buffer.BlockCopy(hashedPassword, this.SaltLength, pbkdf2Subkey, 0, this.Pbkdf2SubkeyLength);

            return pbkdf2Subkey;
        }
    }
}