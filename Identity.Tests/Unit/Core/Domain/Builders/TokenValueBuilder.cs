using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class TokenValueBuilder
    {
        public static readonly Guid DefaultId = Guid.NewGuid();
        public static readonly ApplicationId DefaultApplicationId = ApplicationId.Generate();

        public static TokenValue DefaultTokenValue => new TokenValueBuilder().Build();

        public Guid Id { get; private set; } = TokenValueBuilder.DefaultId;
        public ApplicationId ApplicationId { get; private set; } = TokenValueBuilder.DefaultApplicationId;
        public TokenType Type { get; private set; } = TokenType.Access;

        public IEnumerable<PermissionId> Permissions { get; private set; } = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource"), "Add"),
            new PermissionId(new ResourceId("MyResource"), "Remove")
        };

        public DateTime? ExpirationDate { get; private set; }

        public TokenValueBuilder WithId(Guid guid)
        {
            this.Id = guid;

            return this;
        }

        public TokenValueBuilder WithApplicationId(ApplicationId applicationId)
        {
            this.ApplicationId = applicationId;

            return this;
        }

        public TokenValueBuilder WithType(TokenType type)
        {
            this.Type = type;

            return this;
        }

        public TokenValueBuilder WithPermissions(PermissionId[] permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public TokenValueBuilder WithExpirationDate(DateTime? expirationDate)
        {
            this.ExpirationDate = expirationDate;

            return this;
        }

        public TokenValue Build()
            => TokenValueEncoder.Encode(new TokenInformation(
                this.Id,
                this.ApplicationId,
                this.Type,
                this.Permissions,
                this.ExpirationDate));
    }
}