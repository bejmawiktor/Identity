using DDD.Domain.Events;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Events
{
    public class UserPermissionRevoked : Event
    {
        public Guid UserId { get; }
        public string RevokedPermissionId { get; }

        internal UserPermissionRevoked(
            UserId userId,
            PermissionId revokedPermissionId)
        {
            this.UserId = userId.ToGuid();
            this.RevokedPermissionId = revokedPermissionId.ToString();
        }
    }
}