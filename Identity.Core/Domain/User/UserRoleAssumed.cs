using DDD.Domain.Events;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Events
{
    public class UserRoleAssumed : Event
    {
        public Guid UserId { get; }
        public Guid AssumedRoleId { get; }

        internal UserRoleAssumed(UserId userId, RoleId assumedRoleId)
        {
            this.UserId = userId.ToGuid();
            this.AssumedRoleId = assumedRoleId.ToGuid();
        }
    }
}