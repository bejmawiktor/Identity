using DDD.Domain.Events;

namespace Identity.Domain
{
    public class PermissionCreated : Event
    {
        public PermissionId PermissionId { get; }
        public string PermissionDescription { get; }

        internal PermissionCreated(
            PermissionId permissionId,
            string permissionDescription)
        {
            this.PermissionId = permissionId;
            this.PermissionDescription = permissionDescription;
        }
    }
}