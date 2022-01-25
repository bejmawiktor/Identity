using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class RolePermissionRevokedBuilder
    {
        private static readonly RoleId DefaultId = RoleId.Generate();

        public RoleId RoleId { get; private set; }
            = RolePermissionRevokedBuilder.DefaultId;

        public PermissionId RevokedPermissionId { get; private set; }
            = new PermissionId(new ResourceId("MyResource"), "Permission");

        public RolePermissionRevokedBuilder WithRoleId(RoleId roleId)
        {
            this.RoleId = roleId;

            return this;
        }

        public RolePermissionRevokedBuilder WithRevokedPermissionId(PermissionId revokedPermissionId)
        {
            this.RevokedPermissionId = revokedPermissionId;

            return this;
        }

        public RolePermissionRevoked Build()
            => new RolePermissionRevoked(
                this.RoleId,
                this.RevokedPermissionId);
    }
}