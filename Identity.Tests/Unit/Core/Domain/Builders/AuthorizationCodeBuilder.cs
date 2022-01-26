using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class AuthorizationCodeBuilder
    {
        private static readonly AuthorizationCodeId DefaultId = AuthorizationCodeId.Generate(
            ApplicationId.Generate(),
            out _);

        private static readonly DateTime DefaultExpiresAt = DateTime.Now;

        public static AuthorizationCode DefaultAuthorizationCode => new AuthorizationCodeBuilder().Build();

        public AuthorizationCodeId Id { get; private set; } = AuthorizationCodeBuilder.DefaultId;
        public DateTime ExpiresAt { get; private set; } = AuthorizationCodeBuilder.DefaultExpiresAt;
        public bool Used { get; private set; } = false;

        public IEnumerable<PermissionId> Permissions { get; private set; } = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource"), "Add")
        };

        public AuthorizationCodeBuilder WithId(AuthorizationCodeId id)
        {
            this.Id = id;

            return this;
        }

        public AuthorizationCodeBuilder WithExpiresAt(DateTime expiresAt)
        {
            this.ExpiresAt = expiresAt;

            return this;
        }

        public AuthorizationCodeBuilder WithUsed(bool used)
        {
            this.Used = used;

            return this;
        }

        public AuthorizationCodeBuilder WithPermissions(IEnumerable<PermissionId> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public AuthorizationCode Build()
            => new AuthorizationCode(
                this.Id,
                this.ExpiresAt,
                this.Used,
                this.Permissions);
    }
}