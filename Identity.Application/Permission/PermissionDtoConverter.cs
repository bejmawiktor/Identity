using DDD.Application.Model.Converters;
using Identity.Domain;
using System;

namespace Identity.Application
{
    internal class PermissionDtoConverter : IAggregateRootDtoConverter<Permission, PermissionId, PermissionDto, (string ResourceId, string Name)>
    {
        public PermissionDto ToDto(Permission permission)
        {
            if(permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }

            return new PermissionDto(
                name: permission.Id.Name,
                resourceId: permission.Id.ResourceId.ToString(),
                description: permission.Description);
        }

        public (string ResourceId, string Name) ToDtoIdentifier(PermissionId permissionId)
        {
            if(permissionId == null)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }

            return (permissionId.ResourceId.ToString(), permissionId.Name);
        }
    }
}