using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class UserRoleRevokedBuilder
    {
        private static readonly UserId DefaultUserId = UserId.Generate();
        private static readonly RoleId DefaultRoleId = RoleId.Generate();

        public UserId UserId { get; private set; }
            = UserRoleRevokedBuilder.DefaultUserId;

        public RoleId RevokedRoleId { get; private set; }
            = UserRoleRevokedBuilder.DefaultRoleId;

        public UserRoleRevokedBuilder WithUserId(UserId userId)
        {
            this.UserId = userId;

            return this;
        }

        public UserRoleRevokedBuilder WithRevokedRoleId(RoleId revokedRoleId)
        {
            this.RevokedRoleId = revokedRoleId;

            return this;
        }

        public UserRoleRevoked Build()
            => new UserRoleRevoked(
                this.UserId,
                this.RevokedRoleId);
    }
}