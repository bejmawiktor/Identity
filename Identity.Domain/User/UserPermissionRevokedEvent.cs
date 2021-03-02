using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserPermissionRevokedEvent : Event
    {
        public UserId UserId { get; }
        public PermissionId RevokedPermissionId { get; }

        internal UserPermissionRevokedEvent(
            UserId userId,
            PermissionId revokedPermissionId)
        {
            this.UserId = userId;
            this.RevokedPermissionId = revokedPermissionId;
        }
    }
}