using DDD.Domain.Events;
using System;

namespace Identity.Domain
{
    public class RoleCreated : Event
    {
        public Guid RoleId { get; }
        public string RoleName { get; }
        public string RoleDescription { get; }

        internal RoleCreated(
            RoleId roleId,
            string roleName,
            string roleDescription)
        {
            this.RoleId = roleId.ToGuid();
            this.RoleName = roleName;
            this.RoleDescription = roleDescription;
        }
    }
}