using DDD.Domain.Events;

namespace Identity.Domain
{
    public class RoleCreated : Event
    {
        public RoleId RoleId { get; }
        public string RoleName { get; }
        public string RoleDescription { get; }

        internal RoleCreated(
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