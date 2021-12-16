using DDD.Domain.Events;
using System;

namespace Identity.Domain
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