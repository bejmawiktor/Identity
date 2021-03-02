using DDD.Domain.Events;
using DDD.Domain.Model;
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

        public static Resource Create(string name, string description)
        {
            var resource = new Resource(new ResourceId(name), description);

            EventManager.Instance.Notify(new ResourceCreatedEvent(
                resource.Id,
                resource.Description));

            return resource;
        }

        public Permission CreatePermission(string name, string description)
        {
            var permission = new Permission(new PermissionId(this.Id, name), description);

            EventManager.Instance.Notify(new PermissionCreatedEvent(
                permission.Id,
                permission.Description));

            return permission;
        }
    }
}