using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class RolePermissionObtainedBuilder
    {
        private static readonly RoleId DefaultId = RoleId.Generate();

        public RoleId RoleId { get; private set; }
            = RolePermissionObtainedBuilder.DefaultId;

        public PermissionId ObtainedPermissionId { get; private set; }
            = new PermissionId(new ResourceId("MyResource"), "Permission");

        public RolePermissionObtainedBuilder WithRoleId(RoleId roleId)
        {
            this.RoleId = roleId;

            return this;
        }

        public RolePermissionObtainedBuilder WithObtainedPermissionId(PermissionId obtainedPermissionId)
        {
            this.ObtainedPermissionId = obtainedPermissionId;

            return this;
        }

        public RolePermissionObtained Build()
            => new RolePermissionObtained(
                this.RoleId,
                this.ObtainedPermissionId);
    }
}