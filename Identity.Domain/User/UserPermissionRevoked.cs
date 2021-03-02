using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserPermissionRevoked : Event
    {
        public UserId UserId { get; }
        public PermissionId RevokedPermissionId { get; }

        internal UserPermissionRevoked(
            UserId userId,
            PermissionId revokedPermissionId)
        {
            this.UserId = userId;
            this.RevokedPermissionId = revokedPermissionId;
        }
    }
}