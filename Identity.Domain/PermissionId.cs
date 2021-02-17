using DDD.Model;
using System;
using System.Text.RegularExpressions;

namespace Identity.Domain
{
    public class PermissionId : Identifier<string, PermissionId>
    {
        public string AlphaNumericPattern => "^[a-zA-Z0-9]*$";

        public PermissionId(string name) : base(name)
        {
        }

        protected override void ValidateValue(string value)
        {
            if(value.Length == 0)
            {
                throw new ArgumentException("Permision id can't be empty.");
            }

            if(!Regex.IsMatch(value, this.AlphaNumericPattern))
            {
                throw new ArgumentException("Permision id must contain only alphanumeric characters without spaces.");
            }
        }

        public override string ToString()
            => this.Value;
    }
}