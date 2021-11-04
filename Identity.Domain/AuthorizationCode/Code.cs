using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Domain
{
    public class Code : ValueObject
    {
        private static int Length => 32;

        private static readonly char[] CharacterSet
             = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public string Value { get; }

        public Code(string value)
        {
            this.ValidateValue(value);

            this.Value = value;
        }

        private void ValidateValue(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException("code");
            }

            if(value.Length == 0)
            {
                throw new ArgumentException("Code can't be empty.");
            }

            if(value.Length != Code.Length)
            {
                throw new ArgumentException("Invalid code given.");
            }
        }

        public static Code Generate()
        {
            byte[] data = new byte[4 * Code.Length];
            StringBuilder result = new StringBuilder(Code.Length);

            using(var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            for(int i = 0; i < Code.Length; i++)
            {
                uint random = BitConverter.ToUInt32(data, i * 4);
                long index = random % Code.CharacterSet.Length;

                result.Append(Code.CharacterSet[index]);
            }

            return new Code(result.ToString());
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Value;
        }

        public override string ToString()
            => this.Value;
    }
}