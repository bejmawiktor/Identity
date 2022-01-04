using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Domain
{
    internal class EncryptedTokenValue : ValueObject, IEquatable<EncryptedTokenValue>
    {
        private IEnumerable<byte> EncryptedValue { get; }

        public EncryptedTokenValue(string base64EncryptedTokenValue)
        {
            this.ValidateEncryptedTokenValue(base64EncryptedTokenValue);

            this.EncryptedValue = Convert.FromBase64String(base64EncryptedTokenValue);
        }

        private void ValidateEncryptedTokenValue(string base64EncryptedTokenValue)
        {
            if(base64EncryptedTokenValue == null)
            {
                throw new ArgumentNullException(nameof(base64EncryptedTokenValue));
            }

            if(base64EncryptedTokenValue.Length == 0)
            {
                throw new ArgumentException("Encrypted token value can't be empty.");
            }

            TokenValueEncrypter.Validate(Convert.FromBase64String(base64EncryptedTokenValue));
        }

        internal EncryptedTokenValue(byte[] encryptedTokenValue)
        {
            this.EncryptedValue = encryptedTokenValue;

            this.ValidateEncryptedTokenValue(encryptedTokenValue);
        }

        private void ValidateEncryptedTokenValue(byte[] encryptedTokenValue)
        {
            if(encryptedTokenValue == null)
            {
                throw new ArgumentNullException(nameof(encryptedTokenValue));
            }

            TokenValueEncrypter.Validate(encryptedTokenValue);
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            foreach(byte encryptedValue in this.EncryptedValue)
            {
                yield return encryptedValue;
            }
        }

        public static EncryptedTokenValue Encrypt(TokenValue tokenValue)
        {
            if(tokenValue == null)
            {
                throw new ArgumentNullException(nameof(tokenValue));
            }

            return TokenValueEncrypter.Encrypt(tokenValue);
        }

        internal TokenValue Decrypt()
            => TokenValueEncrypter.Decrypt(this);

        public override string ToString()
            => Convert.ToBase64String(this.ToByteArray());

        public byte[] ToByteArray()
            => this.EncryptedValue.ToArray();

        public bool Equals(EncryptedTokenValue other)
            => this.Equals((object)other);
    }
}