using DDD.Domain.Model;
using System;
using System.Text.RegularExpressions;

namespace Identity.Domain
{
    public class ResourceId : Identifier<string, ResourceId>
    {
        private string AlphaNumericPattern => "^[a-zA-Z0-9]*$";

        public ResourceId(string name) : base(name)
        {
        }

        protected override void ValidateValue(string value)
        {
            if(value.Length == 0)
            {
                throw new ArgumentException("Resource id can't be empty.");
            }

            if(!Regex.IsMatch(value, this.AlphaNumericPattern))
            {
                throw new ArgumentException("Resource id must contain only alphanumeric characters without spaces.");
            }
        }

        public override string ToString()
            => this.Value;
    }
}