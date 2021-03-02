using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserRoleRevoked : Event
    {
        public UserId UserId { get; }
        public RoleId RevokedRoleId { get; }

        public UserRoleRevoked(UserId userId, RoleId revokedRoleId)
        {
            this.UserId = userId;
            this.RevokedRoleId = revokedRoleId;
        }
    }
}