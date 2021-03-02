using DDD.Domain.Events;

namespace Identity.Domain
{
    public class RolePermissionObtainedEvent : Event
    {
        public RoleId RoleId { get; }
        public PermissionId ObtainedPermissionId { get; }

        internal RolePermissionObtainedEvent(
            RoleId roleId,
            PermissionId obtainedPermissionId)
        {
            this.RoleId = roleId;
            this.ObtainedPermissionId = obtainedPermissionId;
        }
    }
}