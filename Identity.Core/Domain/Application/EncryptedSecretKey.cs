using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Domain
{
    internal class EncryptedSecretKey : ValueObject
    {
        private IEnumerable<byte> EncryptedValue { get; }

        public EncryptedSecretKey(string base64EncryptedSecretKey)
        {
            this.ValidateEncryptedSecretKey(base64EncryptedSecretKey);

            this.EncryptedValue = Convert.FromBase64String(base64EncryptedSecretKey);
        }

        private void ValidateEncryptedSecretKey(string base64EncryptedSecretKey)
        {
            if(base64EncryptedSecretKey == null)
            {
                throw new ArgumentNullException(nameof(base64EncryptedSecretKey));
            }

            if(base64EncryptedSecretKey.Length == 0)
            {
                throw new ArgumentException("Encrypted secret key can't be empty.");
            }

            SecretKeyEncrypter.Validate(Convert.FromBase64String(base64EncryptedSecretKey));
        }

        internal EncryptedSecretKey(byte[] encryptedSecretKey)
        {
            this.EncryptedValue = encryptedSecretKey;

            this.ValidateEncryptedSecretKey(encryptedSecretKey);
        }

        private void ValidateEncryptedSecretKey(byte[] encryptedSecretKey)
        {
            if(encryptedSecretKey == null)
            {
                throw new ArgumentNullException(nameof(encryptedSecretKey));
            }

            SecretKeyEncrypter.Validate(encryptedSecretKey);
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            foreach(byte encryptedValue in this.EncryptedValue)
            {
                yield return encryptedValue;
            }
        }

        public static EncryptedSecretKey Encrypt(SecretKey secretKey)
        {
            if(secretKey == null)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            return SecretKeyEncrypter.Encrypt(secretKey);
        }

        internal SecretKey Decrypt()
            => SecretKeyEncrypter.Decrypt(this);

        public override string ToString()
            => Convert.ToBase64String(this.ToByteArray());

        public byte[] ToByteArray()
            => this.EncryptedValue.ToArray();
    }
}