using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Domain
{
    internal class HashedCode : ValueObject
    {
        private static readonly ICodeHashingAlgorithm CodeHashingAlgorithm
            = new SHA256CodeHashingAlgorithm();

        private IEnumerable<byte> HashedValue { get; }

        public HashedCode(string base64HashedCode)
        {
            this.ValidateHashedCode(base64HashedCode);

            this.HashedValue = Convert.FromBase64String(base64HashedCode);
        }

        private void ValidateHashedCode(string base64HashedCode)
        {
            if(base64HashedCode == null)
            {
                throw new ArgumentNullException(nameof(base64HashedCode));
            }

            if(base64HashedCode.Length == 0)
            {
                throw new ArgumentException("Hashed code can't be empty.");
            }

            HashedCode.CodeHashingAlgorithm.Validate(Convert.FromBase64String(base64HashedCode));
        }

        internal HashedCode(byte[] hashedCode)
        {
            this.HashedValue = hashedCode;

            this.ValidateHashedCode(hashedCode);
        }

        private void ValidateHashedCode(byte[] hashedCode)
        {
            if(hashedCode == null)
            {
                throw new ArgumentNullException(nameof(hashedCode));
            }

            HashedCode.CodeHashingAlgorithm.Validate(hashedCode);
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            foreach(var encryptedValue in this.HashedValue)
            {
                yield return encryptedValue;
            }
        }

        public static HashedCode Hash(Code code)
        {
            if(code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            return new HashedCode(HashedCode.CodeHashingAlgorithm.Hash(code));
        }

        public override string ToString()
            => Convert.ToBase64String(this.ToByteArray());

        public byte[] ToByteArray()
            => this.HashedValue.ToArray();
    }
}