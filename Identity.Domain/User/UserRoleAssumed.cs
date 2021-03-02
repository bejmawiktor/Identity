using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserRoleAssumed : Event
    {
        public UserId UserId { get; }
        public RoleId AssumedRoleId { get; }

        public UserRoleAssumed(UserId userId, RoleId assumedRoleId)
        {
            this.UserId = userId;
            this.AssumedRoleId = assumedRoleId;
        }
    }
}