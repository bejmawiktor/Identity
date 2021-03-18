using DDD.Application.Model;
using Identity.Domain;

namespace Identity.Application
{
    public record PermissionDto : IAggregateRootDto<Permission, PermissionId>
    {
        public (string ResourceId, string Name) Id { get; }
        public string Description { get; }

        public PermissionDto(string resourceId, string name, string description)
        {
            this.Id = (resourceId, name);
            this.Description = description;
        }

        internal Permission ToPermission()
            => new Permission(
                id: new PermissionId(new ResourceId(this.Id.ResourceId), this.Id.Name),
                description: this.Description);

        Permission IDomainObjectDto<Permission>.ToDomainObject()
             => this.ToPermission();
    }
}
