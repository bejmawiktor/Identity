using DDD.Model;
using System;

namespace Identity.Domain
{
    public class Resource : AggregateRoot<ResourceId>
    {
        public string Description { get; }

        public Resource(
            ResourceId id,
            string description)
        : base(id)
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

        public Permission CreatePermission(string name, string description)
            => new Permission(
                new PermissionId(resourceId: this.Id, name: name),
                description);
    }
}