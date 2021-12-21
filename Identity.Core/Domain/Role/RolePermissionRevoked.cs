using DDD.Domain.Events;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Events
{
    public class RolePermissionRevoked : Event
    {
        public Guid RoleId { get; }
        public string RevokedPermissionId { get; }

        internal RolePermissionRevoked(
            RoleId roleId,
            PermissionId revokedPermissionId)
        {
            this.RoleId = roleId.ToGuid();
            this.RevokedPermissionId = revokedPermissionId.ToString();
        }
    }
}