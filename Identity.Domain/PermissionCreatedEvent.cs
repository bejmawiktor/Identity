using DDD.Events;
using System;

namespace Identity.Domain
{
    public class PermissionCreatedEvent : Event
    {
        public PermissionId PermissionId { get; }
        public string Description { get; }

        internal PermissionCreatedEvent(
            PermissionId permissionId,
            string description)
        {
            this.PermissionId = permissionId;
            this.Description = description;
        }
    }
}