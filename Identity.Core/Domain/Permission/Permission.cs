using DDD.Domain.Model;
using System;

namespace Identity.Core.Domain
{
    internal class Permission : AggregateRoot<PermissionId>
    {
        public string Description { get; }

        public Permission(PermissionId id, string description) : base(id)
        {
            this.ValidateDescription(description);

            this.Description = description;
        }

        private void ValidateDescription(string description)
        {
            if(description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if(description.Length == 0)
            {
                throw new ArgumentException("Description can't be empty.");
            }
        }
    }
}