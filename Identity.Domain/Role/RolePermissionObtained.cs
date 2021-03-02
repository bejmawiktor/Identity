using DDD.Domain.Events;

namespace Identity.Domain
{
    public class RolePermissionObtained : Event
    {
        public RoleId RoleId { get; }
        public PermissionId ObtainedPermissionId { get; }

        internal RolePermissionObtained(
            RoleId roleId,
            PermissionId obtainedPermissionId)
        {
            this.RoleId = roleId;
            this.ObtainedPermissionId = obtainedPermissionId;
        }
    }
}