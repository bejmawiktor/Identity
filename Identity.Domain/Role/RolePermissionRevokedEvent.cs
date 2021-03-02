using DDD.Domain.Events;

namespace Identity.Domain
{
    public class RolePermissionRevokedEvent : Event
    {
        public RoleId RoleId { get; }
        public PermissionId RevokedPermissionId { get; }

        internal RolePermissionRevokedEvent(
            RoleId roleId,
            PermissionId revokedPermissionId)
        {
            this.RoleId = roleId;
            this.RevokedPermissionId = revokedPermissionId;
        }
    }
}