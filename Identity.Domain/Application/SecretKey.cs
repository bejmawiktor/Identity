using DDD.Domain.Model;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Identity.Domain
{
    public class SecretKey : ValueObject
    {
        private string Value { get; }

        private static uint Length => 32;

        public SecretKey(string value)
        {
            this.ValidateValue(value);

            this.Value = value;
        }

        private void ValidateValue(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if(value == string.Empty)
            {
                throw new ArgumentException("Secret key can't be empty.");
            }
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Value;
        }

        internal static SecretKey Generate()
        {
            byte[] buffer = new byte[SecretKey.Length];

            using(RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider())
            {
                cryptoRandomDataGenerator.GetBytes(buffer);
            }

            return new SecretKey(WebEncoders.Base64UrlEncode(buffer));
        }

        public override string ToString()
            => this.Value;
    }
}