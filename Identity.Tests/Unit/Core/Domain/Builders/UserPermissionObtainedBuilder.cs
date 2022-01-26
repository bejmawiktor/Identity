using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class UserPermissionObtainedBuilder
    {
        private static readonly UserId DefaultUserId = UserId.Generate();

        public UserId UserId { get; private set; } = UserPermissionObtainedBuilder.DefaultUserId;

        public PermissionId ObtainedPermissionId { get; private set; }
            = new PermissionId(new ResourceId("MyResource"), "Permission");

        public UserPermissionObtainedBuilder WithUserId(UserId userId)
        {
            this.UserId = userId;

            return this;
        }

        public UserPermissionObtainedBuilder WithObtainedPermissionId(PermissionId obtainedPermissionId)
        {
            this.ObtainedPermissionId = obtainedPermissionId;

            return this;
        }

        public UserPermissionObtained Build()
            => new UserPermissionObtained(
                this.UserId,
                this.ObtainedPermissionId);
    }
}