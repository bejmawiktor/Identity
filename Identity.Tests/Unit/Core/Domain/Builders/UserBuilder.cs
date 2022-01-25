using Identity.Core.Domain;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class UserBuilder
    {
        private static readonly HashedPassword DefaultPassword = HashedPassword.Hash(new Password("MyPassword"));
        private static readonly UserId DefaultUserId = UserId.Generate();

        public static User DefaultUser => new UserBuilder().Build();

        public UserId Id { get; private set; } = UserBuilder.DefaultUserId;
        public EmailAddress Email { get; private set; } = new("example@example.com");
        public HashedPassword Password { get; private set; } = UserBuilder.DefaultPassword;
        public IEnumerable<RoleId> Roles { get; private set; }
        public IEnumerable<PermissionId> Permissions { get; private set; }

        public UserBuilder WithId(UserId userId)
        {
            this.Id = userId;

            return this;
        }

        public UserBuilder WithEmail(EmailAddress email)
        {
            this.Email = email;

            return this;
        }

        public UserBuilder WithPassword(HashedPassword password)
        {
            this.Password = password;

            return this;
        }

        public UserBuilder WithRoles(IEnumerable<RoleId> roles)
        {
            this.Roles = roles;

            return this;
        }

        public UserBuilder WithPermissions(IEnumerable<PermissionId> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public User Build()
            => new User(
                this.Id,
                this.Email,
                this.Password,
                this.Roles,
                this.Permissions);
    }
}