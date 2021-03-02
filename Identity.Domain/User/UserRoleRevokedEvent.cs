using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserRoleRevokedEvent : Event
    {
        public UserId UserId { get; }
        public RoleId RevokedRoleId { get; }

        public UserRoleRevokedEvent(UserId userId, RoleId revokedRoleId)
        {
            this.UserId = userId;
            this.RevokedRoleId = revokedRoleId;
        }
    }
}