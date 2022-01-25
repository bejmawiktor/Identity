using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class TokenInformationBuilder
    {
        public static TokenInformation DefaultTokenInformation => new TokenInformationBuilder().Build();
        public readonly static Guid DefaultId = Guid.NewGuid();
        public readonly static ApplicationId DefaultApplicationId = ApplicationId.Generate();

        public Guid Id { get; private set; } = TokenInformationBuilder.DefaultId;
        public ApplicationId ApplicationId { get; private set; } = TokenInformationBuilder.DefaultApplicationId;
        public TokenType Type { get; private set; } = TokenType.Access;

        public IEnumerable<PermissionId> Permissions { get; private set; } = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource"), "Add"),
            new PermissionId(new ResourceId("MyResource"), "Remove")
        };

        public DateTime? ExpirationDate { get; private set; }

        public TokenInformationBuilder WithId(Guid guid)
        {
            this.Id = guid;

            return this;
        }

        public TokenInformationBuilder WithApplicationId(ApplicationId applicationId)
        {
            this.ApplicationId = applicationId;

            return this;
        }

        public TokenInformationBuilder WithType(TokenType type)
        {
            this.Type = type;

            return this;
        }

        public TokenInformationBuilder WithPermissions(PermissionId[] permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public TokenInformationBuilder WithExpirationDate(DateTime? expirationDate)
        {
            this.ExpirationDate = expirationDate;

            return this;
        }

        public TokenInformation Build()
            => new TokenInformation(
                this.Id,
                this.ApplicationId,
                this.Type,
                this.Permissions,
                this.ExpirationDate);
    }
}