using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Core.Domain
{
    internal class Url : ValueObject<string>
    {
        public Url(string value) : base(value)
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
    }
}