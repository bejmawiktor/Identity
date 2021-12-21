using DDD.Domain.Events;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Events
{
    public class UserRoleRevoked : Event
    {
        public Guid UserId { get; }
        public Guid RevokedRoleId { get; }

        internal UserRoleRevoked(UserId userId, RoleId revokedRoleId)
        {
            this.UserId = userId.ToGuid();
            this.RevokedRoleId = revokedRoleId.ToGuid();
        }
    }
}