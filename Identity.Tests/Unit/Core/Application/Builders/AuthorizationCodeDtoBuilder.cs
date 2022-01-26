using Identity.Core.Application;
using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class AuthorizationCodeDtoBuilder
    {
        public static AuthorizationCodeDto DefaultAuthorizationCodeDto
            => new AuthorizationCodeDtoBuilder().Build();

        public static ApplicationId DefaultApplicationId { get; } = Identity.Core.Domain.ApplicationId.Generate();

        public static AuthorizationCodeId DefaultAuthorizationCodeId
            => AuthorizationCodeId.Generate(DefaultApplicationId, out _);

        public static IEnumerable<PermissionId> DefaultPermissions { get; } = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource1"), "Add"),
            new PermissionId(new ResourceId("MyResource2"), "Add")
        };

        public string Code { get; private set; }
            = AuthorizationCodeDtoBuilder.DefaultAuthorizationCodeId.Code.ToString();

        public Guid ApplicationId { get; private set; }
            = AuthorizationCodeDtoBuilder.DefaultAuthorizationCodeId.ApplicationId.ToGuid();

        public DateTime ExpiresAt { get; private set; } = DateTime.Now;
        public bool Used { get; private set; } = false;

        public IEnumerable<(string ResourceId, string Name)> Permissions { get; private set; } = new (string ResourceId, string Name)[]
        {
            ("MyResource1", "Add"),
            ("MyResource2", "Add")
        };

        public AuthorizationCodeDtoBuilder WithCode(string code)
        {
            this.Code = code;

            return this;
        }

        public AuthorizationCodeDtoBuilder WithApplicationId(Guid applicationId)
        {
            this.ApplicationId = applicationId;

            return this;
        }

        public AuthorizationCodeDtoBuilder WithExpiresAt(DateTime expiresAt)
        {
            this.ExpiresAt = expiresAt;

            return this;
        }

        public AuthorizationCodeDtoBuilder WithUsed(bool used)
        {
            this.Used = used;

            return this;
        }

        public AuthorizationCodeDtoBuilder WithPermissions(IEnumerable<(string ResourceId, string Name)> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public AuthorizationCodeDto Build()
            => new AuthorizationCodeDto(
                this.Code,
                this.ApplicationId,
                this.ExpiresAt,
                this.Used,
                this.Permissions);
    }
}