using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    public class Password : ValueObject
    {
        private string Value { get; }

        private static int RequiredPasswordLength => 7;

        public Password(string value)
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

            if(value.Length == 0)
            {
                throw new ArgumentException("Password can't be empty.");
            }

            if(value.Length < Password.RequiredPasswordLength)
            {
                throw new ArgumentException("Password must be longer than 6 characters.");
            }
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Value;
        }

        public override string ToString()
            => this.Value;
    }
}