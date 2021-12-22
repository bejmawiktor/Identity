using Identity.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class TokenIdTest
    {
        [Test]
        public void TestToString_WhenConverting_ThenEncryptedTokenValueGiven()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(this.GetTokenValue());
            var tokenId = new TokenId(encryptedTokenValue);

            Assert.That(tokenId.ToString(), Is.EqualTo(encryptedTokenValue.ToString()));
        }

        private TokenValue GetTokenValue(
            Guid? id = null,
            ApplicationId applicationId = null,
            TokenType tokenType = null,
            IEnumerable<PermissionId> permissions = null,
            DateTime? expirationDate = null)
        {
            var tokenInformation = new TokenInformation(
                id ?? Guid.NewGuid(),
                applicationId ?? ApplicationId.Generate(),
                tokenType ?? TokenType.Access,
                permissions ?? new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add"),
                    new PermissionId(new ResourceId("MyResource"), "Remove")
                },
                expirationDate);

            return TokenValueEncoder.Encode(tokenInformation);
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(this.GetTokenValue(applicationId: applicationId));

            var tokenId = new TokenId(encryptedTokenValue);

            Assert.That(tokenId.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenTypeIsSet()
        {
            TokenValue tokenValue = this.GetTokenValue(tokenType: TokenType.Refresh);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            var tokenId = new TokenId(encryptedTokenValue);

            Assert.That(tokenId.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenExpiresAtIsSet()
        {
            DateTime expirationDate = DateTime.Now;
            TokenValue tokenValue = this.GetTokenValue(expirationDate: expirationDate);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            var tokenId = new TokenId(encryptedTokenValue);

            Assert.That(tokenId.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueGiven_ThenPermissionsAreSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenValue tokenValue = this.GetTokenValue(permissions: permissions);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            var tokenId = new TokenId(encryptedTokenValue);

            Assert.That(tokenId.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestGenerateAccessToken_WhenGenerated_ThenTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
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
            var permissions = new PermissionId[]
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
            var permissions = new PermissionId[]
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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            TokenId tokenid = TokenId.GenerateRefreshTokenId(applicationId, permissions, expiresAt);

            Assert.Multiple(() =>
            {
                Assert.That(tokenid.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokenid.Type, Is.EqualTo(TokenType.Refresh));
                Assert.That(tokenid.ExpiresAt, Is.EqualTo(expiresAt).Within(1).Seconds);
                Assert.That(tokenid.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestGenerateRefreshToken_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            DateTime expiresAt = DateTime.Now;
            var permissions = new PermissionId[]
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
            var permissions = new PermissionId[]
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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            TokenValue tokenValue = this.GetTokenValue(permissions: permissions);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            var tokenId = new TokenId(encryptedTokenValue);

            TokenValue decryptedTokenValue = tokenId.Decrypt();

            Assert.That(decryptedTokenValue, Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestExpired_WhenExpiredTokenGiven_ThenTrueIsReturned()
        {
            var expirationDate = DateTime.Now.AddMinutes(-1);
            TokenValue tokenValue = this.GetTokenValue(expirationDate: expirationDate);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            var tokenId = new TokenId(encryptedTokenValue);

            bool expired = tokenId.Expired;

            Assert.That(expired, Is.True);
        }

        [Test]
        public void TestExpired_WhenValidTokenGiven_ThenFalseIsReturned()
        {
            var expirationDate = DateTime.Now.AddMinutes(1);
            TokenValue tokenValue = this.GetTokenValue(expirationDate: expirationDate);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            var tokenId = new TokenId(encryptedTokenValue);

            bool expired = tokenId.Expired;

            Assert.That(expired, Is.False);
        }
    }
}