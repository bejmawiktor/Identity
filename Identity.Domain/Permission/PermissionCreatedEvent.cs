using DDD.Domain.Events;

namespace Identity.Domain
{
    public class PermissionCreatedEvent : Event
    {
        public PermissionId PermissionId { get; }
        public string PermissionDescription { get; }

        internal PermissionCreatedEvent(
            PermissionId permissionId,
            string permissionDescription)
        {
            this.PermissionId = permissionId;
            this.PermissionDescription = permissionDescription;
        }
    }
}