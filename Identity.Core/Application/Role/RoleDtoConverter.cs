using DDD.Application.Model.Converters;
using Identity.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Application
{
    internal class RoleDtoConverter : IAggregateRootDtoConverter<Role, RoleId, RoleDto, Guid>
    {
        public RoleDto ToDto(Role role)
        {
            if(role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return new RoleDto(
                id: role.Id.ToGuid(),
                name: role.Name,
                description: role.Description,
                permissions: this.ConvertPermissions(role.Permissions));
        }

        private IEnumerable<(string ResourceId, string Name)> ConvertPermissions(IEnumerable<PermissionId> permissions)
        {
            return permissions.Select(p => this.CreatePermissionIdTuple(p));
        }

        private (string ResourceId, string Name) CreatePermissionIdTuple(PermissionId p)
        {
            return (ResourceId: p.ResourceId.ToString(), Name: p.Name);
        }

        public Guid ToDtoIdentifier(RoleId roleId)
        {
            if(roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            return roleId.ToGuid();
        }
    }
}