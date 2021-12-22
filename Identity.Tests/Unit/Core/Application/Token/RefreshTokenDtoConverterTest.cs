using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class RefreshTokenDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenRefreshTokenGiven_ThenRefreshTokenDtoIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            var refreshToken = new RefreshToken(
                tokenId,
                true);
            var refreshTokenDtoConverter = new RefreshTokenDtoConverter();

            RefreshTokenDto refreshTokenDto = refreshTokenDtoConverter.ToDto(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(refreshTokenDto.Id, Is.EqualTo(refreshToken.Id.ToString()));
                Assert.That(refreshTokenDto.Used, Is.EqualTo(refreshToken.Used));
            });
        }

        [Test]
        public void TestToDto_WhenNullRefreshTokenGiven_ThenArgumentNullExceptionIsThrown()
        {
            var refreshTokenDtoConverter = new RefreshTokenDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("refreshToken"),
                () => refreshTokenDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenTokenIdGiven_ThenDtoIdentifierIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            var refreshTokenDtoConverter = new RefreshTokenDtoConverter();

            string refreshTokenDtoId = refreshTokenDtoConverter.ToDtoIdentifier(tokenId);

            Assert.That(refreshTokenDtoId, Is.EqualTo(tokenId.ToString()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullTokenIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var refreshTokenDtoConverter = new RefreshTokenDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenId"),
                () => refreshTokenDtoConverter.ToDtoIdentifier(null));
        }
    }
}
