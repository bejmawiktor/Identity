using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class RoleCreatedBuilder
    {
        private static readonly RoleId DefaultRoleId = RoleId.Generate();

        public RoleId RoleId { get; private set; } = RoleCreatedBuilder.DefaultRoleId;
        public string RoleName { get; private set; } = "RoleName";
        public string RoleDescription { get; private set; } = "Test role description";

        public RoleCreatedBuilder WithRoleId(RoleId roleId)
        {
            this.RoleId = roleId;

            return this;
        }

        public RoleCreatedBuilder WithRoleName(string roleName)
        {
            this.RoleName = roleName;

            return this;
        }

        public RoleCreatedBuilder WithRoleDescription(string roleDescription)
        {
            this.RoleDescription = roleDescription;

            return this;
        }

        public RoleCreated Build()
            => new RoleCreated(
                this.RoleId,
                this.RoleName,
                this.RoleDescription);
    }
}