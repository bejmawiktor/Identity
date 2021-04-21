using DDD.Application.Model;
using Identity.Domain;
using System;

namespace Identity.Application
{
    public class PermissionDto : IAggregateRootDto<Permission, PermissionId>
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

        public override bool Equals(object obj)
        {
            return obj is PermissionDto dto
                && this.Id.Equals(dto.Id)
                && this.Description == dto.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Description);
        }
    }
}