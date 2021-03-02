using DDD.Domain.Events;

namespace Identity.Domain
{
    public class RolePermissionRevoked : Event
    {
        public RoleId RoleId { get; }
        public PermissionId RevokedPermissionId { get; }

        internal RolePermissionRevoked(
            RoleId roleId,
            PermissionId revokedPermissionId)
        {
            this.RoleId = roleId;
            this.RevokedPermissionId = revokedPermissionId;
        }
    }
}