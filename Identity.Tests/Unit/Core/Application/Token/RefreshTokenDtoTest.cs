using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class RefreshTokenDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto tokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .Build();

            Assert.That(tokenDto.Id, Is.EqualTo(tokenId.ToString()));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            RefreshTokenDto tokenDto = new RefreshTokenDtoBuilder()
                .WithUsed(true)
                .Build();

            Assert.That(tokenDto.Used, Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalTokensDtosGiven_ThenTrueIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(true)
                .Build();
            RefreshTokenDto rightTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(true)
                .Build();

            Assert.That(leftTokenDto.Equals(rightTokenDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentTokensDtosGiven_ThenFalseIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(true)
                .Build();
            RefreshTokenDto rightTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(false)
                .Build();

            Assert.That(leftTokenDto.Equals(rightTokenDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdentitcalTokensDtosGiven_ThenSameHashCodesAreReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(true)
                .Build();
            RefreshTokenDto rightTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(true)
                .Build();

            Assert.That(leftTokenDto.GetHashCode(), Is.EqualTo(rightTokenDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentTokensDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions);
            RefreshTokenDto leftTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(true)
                .Build();
            RefreshTokenDto rightTokenDto = new RefreshTokenDtoBuilder()
                .WithId(tokenId.ToString())
                .WithUsed(false)
                .Build();

            Assert.That(leftTokenDto.GetHashCode(), Is.Not.EqualTo(rightTokenDto.GetHashCode()));
        }

        [Test]
        public void TestToRefreshToken_WhenConvertingToToken_ThenTokenIsReturned()
        {
            RefreshTokenDto tokenDto = RefreshTokenDtoBuilder.DefaultRefreshTokenDto;

            RefreshToken refreshToken = tokenDto.ToRefreshToken();

            Assert.Multiple(() =>
            {
                Assert.That(refreshToken.Id, Is.EqualTo(new TokenId(new EncryptedTokenValue(tokenDto.Id))));
                Assert.That(refreshToken.Used, Is.False);
            });
        }
    }
}