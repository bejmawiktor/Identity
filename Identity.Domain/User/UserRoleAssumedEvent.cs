using DDD.Events;

namespace Identity.Domain
{
    public class UserRoleAssumedEvent : Event
    {
        public UserId UserId { get; }
        public RoleId AssumedRoleId { get; }

        public UserRoleAssumedEvent(UserId userId, RoleId assumedRoleId)
        {
            this.UserId = userId;
            this.AssumedRoleId = assumedRoleId;
        }
    }
}