using DDD.Domain.Model;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Identity.Core.Domain
{
    internal class SecretKey : ValueObject<string>
    {
        private static uint Length => 32;

        public SecretKey(string value) : base(value)
        {
        }

        protected override void ValidateValue(string value)
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

        internal static SecretKey Generate()
        {
            byte[] buffer = new byte[SecretKey.Length];

            using(RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider())
            {
                cryptoRandomDataGenerator.GetBytes(buffer);
            }

            return new SecretKey(WebEncoders.Base64UrlEncode(buffer));
        }
    }
}