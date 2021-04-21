using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    public class Url : ValueObject
    {
        private string Value { get; }

        public Url(string value)
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
                throw new ArgumentException("Url can't be empty.");
            }

            System.Uri uri = null;

            if(!System.Uri.TryCreate(value, UriKind.Absolute, out uri))
            {
                throw new ArgumentException("Invalid url given.");
            }

            if(!(uri.Scheme == "http" || uri.Scheme == "https"))
            {
                throw new ArgumentException("Invalid url given.");
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