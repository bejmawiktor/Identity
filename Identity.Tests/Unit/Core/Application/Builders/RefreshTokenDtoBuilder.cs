using Identity.Core.Application;
using Identity.Core.Domain;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class RefreshTokenDtoBuilder
    {
        private static readonly IEnumerable<PermissionId> DefaultPermissions = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource"), "Add"),
            new PermissionId(new ResourceId("MyResource"), "Remove")
        };

        private static readonly string DefaultId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), DefaultPermissions)
            .ToString();

        public static RefreshTokenDto DefaultRefreshTokenDto => new RefreshTokenDtoBuilder().Build();

        public string Id { get; private set; } = RefreshTokenDtoBuilder.DefaultId;
        public bool Used { get; private set; } = false;

        public RefreshTokenDtoBuilder WithId(string id)
        {
            this.Id = id;

            return this;
        }

        public RefreshTokenDtoBuilder WithUsed(bool used)
        {
            this.Used = used;

            return this;
        }

        public RefreshTokenDto Build()
            => new RefreshTokenDto(
                this.Id,
                this.Used);
    }
}