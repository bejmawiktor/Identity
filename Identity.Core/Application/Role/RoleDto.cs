using DDD.Application.Model;
using Identity.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Application
{
    public class RoleDto : IAggregateRootDto<Role, RoleId>
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

        internal Role ToRole()
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

        public override bool Equals(object obj)
        {
            return obj is RoleDto dto
                && this.Id.Equals(dto.Id)
                && this.Name == dto.Name
                && this.Description == dto.Description
                && this.Permissions.SequenceEqual(dto.Permissions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + this.Id.GetHashCode();
                hash = hash * 23 + this.Name?.GetHashCode() ?? 0;
                hash = hash * 23 + this.Description?.GetHashCode() ?? 0;

                foreach((string ResourceId, string Name) permission in this.Permissions)
                {
                    hash = hash * 23 + permission.GetHashCode();
                }

                return hash;
            }
        }
    }
}