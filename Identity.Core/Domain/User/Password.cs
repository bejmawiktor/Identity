using DDD.Domain.Model;
using System;

namespace Identity.Core.Domain
{
    internal class Password : ValueObject<string>
    {
        private static int RequiredPasswordLength => 7;

        public Password(string value) : base(value)
        {
        }

        protected override void ValidateValue(string value)
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
    }
}