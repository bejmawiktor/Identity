using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;
    using RefreshToken = Identity.Persistence.MSSQL.DataModels.RefreshToken;

    [TestFixture]
    public class RefreshTokenTest
    {
        [Test]
        public void TestConstructor_WhenDtoGiven_ThenMembersAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            DateTime expiresAt = DateTime.Now;
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions, expiresAt);
            RefreshToken refreshToken = new(
                new RefreshTokenDto(tokenId.ToString(), true));

            Assert.Multiple(() =>
            {
                Assert.That(refreshToken.Id, Is.EqualTo(tokenId.ToString()));
                Assert.That(refreshToken.Used, Is.True);
            });
        }

        [Test]
        public void TestSetFields_WhenDtoGiven_ThenMembersAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            DateTime expiresAt = DateTime.Now;
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions, expiresAt);
            RefreshToken refreshToken = new();

            refreshToken.SetFields(new RefreshTokenDto(tokenId.ToString(), true));

            Assert.Multiple(() =>
            {
                Assert.That(refreshToken.Id, Is.EqualTo(tokenId.ToString()));
                Assert.That(refreshToken.Used, Is.True);
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenRefreshTokenDtoIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            DateTime expiresAt = DateTime.Now;
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions, expiresAt);
            RefreshToken refreshToken = new RefreshToken()
            {
                Id = tokenId.ToString(),
                Used = true
            };

            RefreshTokenDto refreshTokenDto = refreshToken.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(refreshTokenDto.Id, Is.EqualTo(tokenId.ToString()));
                Assert.That(refreshTokenDto.Used, Is.True);
            });
        }
    }
}