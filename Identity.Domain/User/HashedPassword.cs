using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain
{
    public class HashedPassword : ValueObject
    {
        private IEnumerable<byte> HashedValue { get; }
        private static int RequiredPasswordLength => 7;

        public HashedPassword(string base64HashedPassword)
        {
            this.ValidateHashedPassword(base64HashedPassword);

            this.HashedValue = Convert.FromBase64String(base64HashedPassword);
        }

        private void ValidateHashedPassword(string hashedPassword)
        {
            if(hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if(hashedPassword.Length == 0)
            {
                throw new ArgumentException("Hashed password can't be empty.");
            }

            PasswordHasher.Validate(Convert.FromBase64String(hashedPassword));
        }

        internal HashedPassword(byte[] hashedPassword)
        {
            this.ValidateHashedPassword(hashedPassword);

            this.HashedValue = hashedPassword;
        }

        private void ValidateHashedPassword(byte[] hashedPassword)
        {
            if(hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            PasswordHasher.Validate(hashedPassword);
        }

        public static HashedPassword Hash(string password)
        {
            if(password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if(password.Length == 0)
            {
                throw new ArgumentException("Password can't be empty.");
            }

            if(password.Length < HashedPassword.RequiredPasswordLength)
            {
                throw new ArgumentException("Password must be longer than 6 characters.");
            }

            return PasswordHasher.Hash(password);
        }

        public PasswordVerificationResult Verify(string verifiedPassword)
        {
            if(verifiedPassword == null)
            {
                throw new ArgumentNullException(nameof(verifiedPassword));
            }

            if(verifiedPassword.Length == 0)
            {
                throw new ArgumentException("Verified password can't be empty.");
            }

            return PasswordHasher.Verify(this, verifiedPassword);
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            foreach(var @byte in this.HashedValue)
            {
                yield return @byte;
            }
        }

        public override string ToString()
            => Convert.ToBase64String(this.ToByteArray());

        public byte[] ToByteArray()
            => this.HashedValue.ToArray();
    }
}