using DDD.Domain.Events;
using System;

namespace Identity.Domain
{
    public class UserPermissionObtained : Event
    {
        public Guid UserId { get; }
        public string ObtainedPermissionId { get; }

        internal UserPermissionObtained(
            UserId userId,
            PermissionId obtainedPermissionId)
        {
            this.UserId = userId.ToGuid();
            this.ObtainedPermissionId = obtainedPermissionId.ToString();
        }
    }
}