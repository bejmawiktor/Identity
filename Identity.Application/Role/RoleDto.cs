using DDD.Application.Model;
using Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Application
{
    public record RoleDto : IAggregateRootDto<Role, RoleId>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public IEnumerable<(string ResourceId, string Name)> Permissions { get; }

        public RoleDto(
            Guid id,
            string name,
            string description,
            IEnumerable<(string ResourceId, string Name)> permissions = null)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Permissions = permissions ?? Enumerable.Empty<(string ResourceId, string Name)>();
        }

        public Role ToRole()
            => new Role(
                id: new RoleId(this.Id),
                name: this.Name,
                description: this.Description,
                permissions: this.ConvertPermissions());

        private IEnumerable<PermissionId> ConvertPermissions()
            => this.Permissions.Select(p => this.CreatePermissionId(p));

        private PermissionId CreatePermissionId((string ResourceId, string Name) permissionIdTuple)
            => new PermissionId(new ResourceId(permissionIdTuple.ResourceId), permissionIdTuple.Name);

        Role IDomainObjectDto<Role>.ToDomainObject()
            => this.ToRole();
    }
}