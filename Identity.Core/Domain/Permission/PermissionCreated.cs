using DDD.Domain.Events;
using Identity.Core.Domain;

namespace Identity.Core.Events
{
    public class PermissionCreated : Event
    {
        public string PermissionId { get; }
        public string PermissionDescription { get; }

        internal PermissionCreated(
            PermissionId permissionId,
            string permissionDescription)
        {
            this.PermissionId = permissionId.ToString();
            this.PermissionDescription = permissionDescription;
        }
    }
}