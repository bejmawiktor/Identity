using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class UserPermissionRevokedBuilder
    {
        private static readonly UserId DefaultUserId = UserId.Generate();

        public UserId UserId { get; private set; } = UserPermissionRevokedBuilder.DefaultUserId;
        public PermissionId RevokedPermissionId { get; private set; }
            = new PermissionId(new ResourceId("MyResource"), "Permission");

        public UserPermissionRevokedBuilder WithUserId(UserId userId)
        {
            this.UserId = userId;

            return this;
        }

        public UserPermissionRevokedBuilder WithRevokedPermissionId(PermissionId revokedPermissionId)
        {
            this.RevokedPermissionId = revokedPermissionId;

            return this;
        }

        public UserPermissionRevoked Build()
            => new UserPermissionRevoked(
                this.UserId,
                this.RevokedPermissionId);
    }
}