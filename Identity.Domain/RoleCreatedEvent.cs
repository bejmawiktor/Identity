using DDD.Events;
using System;

namespace Identity.Domain
{
    public class RoleCreatedEvent : Event
    {
        public RoleId RoleId { get; }
        public string RoleName { get; }
        public string RoleDescription { get; }

        internal RoleCreatedEvent(
            RoleId roleId,
            string roleName,
            string roleDescription)
        {
            this.RoleId = roleId;
            this.RoleName = roleName;
            this.RoleDescription = roleDescription;
        }
    }
}