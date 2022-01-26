using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class UserRoleAssumedBuilder
    {
        private static readonly UserId DefaultUserId = UserId.Generate();
        private static readonly RoleId DefaultRoleId = RoleId.Generate();

        public UserId UserId { get; private set; }
            = UserRoleAssumedBuilder.DefaultUserId;

        public RoleId AssumedRoleId { get; private set; }
            = UserRoleAssumedBuilder.DefaultRoleId;

        public UserRoleAssumedBuilder WithUserId(UserId userId)
        {
            this.UserId = userId;

            return this;
        }

        public UserRoleAssumedBuilder WithAssumedRoleId(RoleId assumedRoleId)
        {
            this.AssumedRoleId = assumedRoleId;

            return this;
        }

        public UserRoleAssumed Build()
            => new UserRoleAssumed(
                this.UserId,
                this.AssumedRoleId);
    }
}