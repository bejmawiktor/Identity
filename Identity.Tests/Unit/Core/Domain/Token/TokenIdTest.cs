using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class TokenIdTest
    {
        [Test]
        public void TestToString_WhenConverting_ThenEncryptedTokenValueGiven()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(TokenValueBuilder.DefaultTokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            Assert.That(tokenId.ToString(), Is.EqualTo(encryptedTokenValue.ToString()));
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue tokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            TokenId tokenId = new(encryptedTokenValue);

            Assert.That(tokenId.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenTypeIsSet()
        {
            TokenValue tokenValue = new TokenValueBuilder()
                .WithType(TokenType.Refresh)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            TokenId tokenId = new(encryptedTokenValue);

            Assert.That(tokenId.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenExpiresAtIsSet()
        {
            DateTime expirationDate = DateTime.Now;
            TokenValue tokenValue = new TokenValueBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            TokenId tokenId = new(encryptedTokenValue);

            Assert.That(tokenId.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenPermissionsAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource2"), "Add"),
                new PermissionId(new ResourceId("MyResource2"), "Remove")
            };
            TokenValue tokenValue = new TokenValueBuilder()
                .WithPermissions(permissions)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            TokenId tokenId = new(encryptedTokenValue);

            Assert.That(tokenId.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestGenerateAccessToken_WhenGenerated_ThenTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            TokenId tokenid = TokenId.GenerateAccessTokenId(applicationId, permissions);

            Assert.Multiple(() =>
            {
                Assert.That(tokenid.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokenid.Type, Is.EqualTo(TokenType.Access));
                Assert.That(tokenid.ExpiresAt, Is.EqualTo(DateTime.Now.AddDays(1)).Within(1).Hours);
                Assert.That(tokenid.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestGenerateAccessToken_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationId"),
                () => TokenId.GenerateAccessTokenId(null, permissions));
        }

        [Test]
        public void TestGenerateAccessToken_WhenNullPermissionsGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissions"),
                () => TokenId.GenerateAccessTokenId(ApplicationId.Generate(), null));
        }

        [Test]
        public void TestGenerateRefreshToken_WhenGenerated_ThenTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            DateTime expiresAt = DateTime.Now;
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            TokenId tokenId = TokenId.GenerateRefreshTokenId(applicationId, permissions, expiresAt);

            Assert.Multiple(() =>
            {
                Assert.That(tokenId.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokenId.Type, Is.EqualTo(TokenType.Refresh));
                Assert.That(tokenId.ExpiresAt, Is.EqualTo(expiresAt).Within(1).Seconds);
                Assert.That(tokenId.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestGenerateRefreshToken_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            DateTime expiresAt = DateTime.Now;
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationId"),
                () => TokenId.GenerateRefreshTokenId(null, permissions, expiresAt));
        }

        [Test]
        public void TestGenerateRefreshToken_WhenNullPermissionsdGiven_ThenArgumentNullExceptionIsThrown()
        {
            DateTime expiresAt = DateTime.Now;
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissions"),
                () => TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), null, expiresAt));
        }

        [Test]
        public void TestDecrypt_WhenDecrypting_ThenTokenValueIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenValue tokenValue = new TokenValueBuilder()
                .WithPermissions(permissions)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            TokenValue decryptedTokenValue = tokenId.Decrypt();

            Assert.That(decryptedTokenValue, Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestExpired_WhenExpiredTokenGiven_ThenTrueIsReturned()
        {
            DateTime expirationDate = DateTime.Now.AddMinutes(-1);
            TokenValue tokenValue = new TokenValueBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            bool expired = tokenId.Expired;

            Assert.That(expired, Is.True);
        }

        [Test]
        public void TestExpired_WhenValidTokenGiven_ThenFalseIsReturned()
        {
            DateTime expirationDate = DateTime.Now.AddMinutes(1);
            TokenValue tokenValue = new TokenValueBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            bool expired = tokenId.Expired;

            Assert.That(expired, Is.False);
        }
    }
}