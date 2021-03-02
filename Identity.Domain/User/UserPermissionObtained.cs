using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserPermissionObtained : Event
    {
        public UserId UserId { get; }
        public PermissionId ObtainedPermissionId { get; }

        internal UserPermissionObtained(
            UserId userId,
            PermissionId obtainedPermissionId)
        {
            this.UserId = userId;
            this.ObtainedPermissionId = obtainedPermissionId;
        }
    }
}