using DDD.Domain.Model;
using System;
using System.Text.RegularExpressions;

namespace Identity.Core.Domain
{
    internal class EmailAddress : ValueObject<string>
    {
        private string CorrectAddressPattern
            => @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        public EmailAddress(string value) : base(value)
        {
        }

        protected override void ValidateValue(string address)
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if(address.Length == 0)
            {
                throw new ArgumentException("Email address can't be empty.");
            }

            if(!Regex.IsMatch(address, this.CorrectAddressPattern))
            {
                throw new ArgumentException("Incorrect email address given.");
            }
        }
    }
}