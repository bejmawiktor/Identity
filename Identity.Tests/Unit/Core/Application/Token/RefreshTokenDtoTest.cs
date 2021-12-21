﻿using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class RefreshTokenDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto tokenDto = this.GetRefreshTokenDto(id: tokenId.ToString());

            Assert.That(tokenDto.Id, Is.EqualTo(tokenId.ToString()));
        }

        private RefreshTokenDto GetRefreshTokenDto(
            string id = null,
            bool? used = false)
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            
            return new RefreshTokenDto(
                id ?? tokenId.ToString(),
                used ?? false);
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            RefreshTokenDto tokenDto = this.GetRefreshTokenDto(used: true);

            Assert.That(tokenDto.Used, Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalTokensDtosGiven_ThenTrueIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDto(tokenId.ToString(), true);
            RefreshTokenDto rightTokenDto = new RefreshTokenDto(tokenId.ToString(), true);

            Assert.That(leftTokenDto.Equals(rightTokenDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentTokensDtosGiven_ThenFalseIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDto(tokenId.ToString(), true);
            RefreshTokenDto rightTokenDto = new RefreshTokenDto(tokenId.ToString(), false);

            Assert.That(leftTokenDto.Equals(rightTokenDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdentitcalTokensDtosGiven_ThenSameHashCodesAreReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDto(tokenId.ToString(), true);
            RefreshTokenDto rightTokenDto = new RefreshTokenDto(tokenId.ToString(), true);

            Assert.That(leftTokenDto.GetHashCode(), Is.EqualTo(rightTokenDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentTokensDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDto(tokenId.ToString(), true);
            RefreshTokenDto rightTokenDto = new RefreshTokenDto(tokenId.ToString(), false);

            Assert.That(leftTokenDto.GetHashCode(), Is.Not.EqualTo(rightTokenDto.GetHashCode()));
        }

        [Test]
        public void TestToRefreshToken_WhenConvertingToToken_ThenTokenIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto tokenDto = new RefreshTokenDto(
                tokenId.ToString(),
                true);

            RefreshToken refreshToken = tokenDto.ToRefreshToken();

            Assert.Multiple(() =>
            {
                Assert.That(refreshToken.Id, Is.EqualTo(tokenId));
                Assert.That(refreshToken.Used, Is.True);
            });
        }
    }
}
