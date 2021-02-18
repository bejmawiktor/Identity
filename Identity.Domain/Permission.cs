using DDD.Model;
using System;

namespace Identity.Domain
{
    public class Permission : Entity<PermissionId>
    {
        public string Description { get; }

        public Permission(PermissionId id, string description) : base(id)
        {
            this.ValidateMembers(description);

            this.Description = description;
        }

        private void ValidateMembers(string description)
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