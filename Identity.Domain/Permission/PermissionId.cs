using DDD.Model;
using System;
using System.Text.RegularExpressions;

namespace Identity.Domain
{
    public class PermissionId : CompositeIdentifier<(ResourceId ResourceId, string Name), PermissionId>
    {
        public string Name => this.Value.Name;
        public ResourceId ResourceId => this.Value.ResourceId;
        private string AlphaNumericPattern => "^[a-zA-Z0-9]*$";

        public PermissionId(ResourceId resourceId, string name) : base((resourceId, name))
        {
        }

        protected override void ValidateValue((ResourceId ResourceId, string Name) value)
        {
            if(value.ResourceId == null)
            {
                throw new ArgumentNullException("resourceId");
            }

            if(value.Name == null)
            {
                throw new ArgumentNullException("name");
            }

            if(value.Name.Length == 0)
            {
                throw new ArgumentException("Name can't be empty.");
            }

            if(!Regex.IsMatch(value.Name, this.AlphaNumericPattern))
            {
                throw new ArgumentException("Name must contain only alphanumeric characters without spaces.");
            }
        }

        public override string ToString()
            => $"{this.ResourceId}.{this.Name}";
    }
}