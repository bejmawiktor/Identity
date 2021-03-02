using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserPermissionObtainedEvent : Event
    {
        public UserId UserId { get; }
        public PermissionId ObtainedPermissionId { get; }

        internal UserPermissionObtainedEvent(
            UserId userId,
            PermissionId obtainedPermissionId)
        {
            this.UserId = userId;
            this.ObtainedPermissionId = obtainedPermissionId;
        }
    }
}