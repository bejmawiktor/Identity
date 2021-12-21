using DDD.Domain.Events;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Events
{
    public class RolePermissionObtained : Event
    {
        public Guid RoleId { get; }
        public string ObtainedPermissionId { get; }

        internal RolePermissionObtained(
            RoleId roleId,
            PermissionId obtainedPermissionId)
        {
            this.RoleId = roleId.ToGuid();
            this.ObtainedPermissionId = obtainedPermissionId.ToString();
        }
    }
}