using DDD.Domain.Events;
using System;

namespace Identity.Domain
{
    public class UserRoleAssumed : Event
    {
        public Guid UserId { get; }
        public Guid AssumedRoleId { get; }

        public UserRoleAssumed(UserId userId, RoleId assumedRoleId)
        {
            this.UserId = userId.ToGuid();
            this.AssumedRoleId = assumedRoleId.ToGuid();
        }
    }
}