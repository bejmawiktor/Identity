using DDD.Domain.Events;
using System;

namespace Identity.Domain
{
    public class UserRoleRevoked : Event
    {
        public Guid UserId { get; }
        public Guid RevokedRoleId { get; }

        public UserRoleRevoked(UserId userId, RoleId revokedRoleId)
        {
            this.UserId = userId.ToGuid();
            this.RevokedRoleId = revokedRoleId.ToGuid();
        }
    }
}