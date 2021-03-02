using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Identity.Domain
{
    public class EmailAddress : ValueObject
    {
        private string Address { get; }

        private string CorrectAddressPattern
            => @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        public EmailAddress(string address)
        {
            this.ValidateAddress(address);

            this.Address = address;
        }

        protected void ValidateAddress(string address)
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

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Address;
        }

        public override string ToString()
            => this.Address;
    }
}