using Identity.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class TokenTest
    {
        [Test]
        public void TestToString_WhenTokenIdGiven_ThenValueIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(this.GetTokenValue(applicationId: applicationId));
            var tokenId = new TokenId(encryptedTokenValue);

            var token = new TokenStub(tokenId);

            Assert.That(token.ToString(), Is.EqualTo(tokenId.ToString()));
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
        public void TestConstructor_WhenTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(this.GetTokenValue(applicationId: applicationId));
            var tokenId = new TokenId(encryptedTokenValue);

            var token = new TokenStub(tokenId);

            Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenTypeIsSet()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue(tokenType: TokenType.Refresh));
            var tokenId = new TokenId(encryptedTokenValue);

            var token = new TokenStub(tokenId);

            Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenExpiresAtIsSet()
        {
            DateTime expirationDate = DateTime.Now;
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue(expirationDate: expirationDate));
            var tokenId = new TokenId(encryptedTokenValue);

            var token = new TokenStub(tokenId);

            Assert.That(token.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenPermissionsAreSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue(permissions: permissions));
            var tokenId = new TokenId(encryptedTokenValue);

            var token = new TokenStub(tokenId);

            Assert.That(token.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestVerify_WhenExtraVerificationReturnedFailedAndTokenExpired_ThenTokenVerificationFailedIsReturnedWithExtraVerificationMessage()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue());
            var tokenId = new TokenId(encryptedTokenValue);
            var token = new TokenStub(tokenId, true);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.Multiple(() =>
            {
                Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Failed));
                Assert.That(tokenVerificationResult.Message, Is.EqualTo("Token has been used."));
            });
        }

        [Test]
        public void TestVerify_WhenExtraVerificationReturnedFailedAndTokenNotExpired_ThenTokenVerificationFailedIsReturned()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue());
            var tokenId = new TokenId(encryptedTokenValue);
            var token = new TokenStub(tokenId, true);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.Multiple(() =>
            {
                Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Failed));
                Assert.That(tokenVerificationResult.Message, Is.EqualTo("Token has been used."));
            });
        }

        [Test]
        public void TestVerify_WhenTokenExpiredAndExtraVerificationReturnedSuccess_ThenTokenVerificationFailedIsReturned()
        {
            DateTime expirationDate = DateTime.Now.AddMinutes(-1);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue(expirationDate: expirationDate));
            var tokenId = new TokenId(encryptedTokenValue);
            var token = new TokenStub(tokenId);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.Multiple(() =>
            {
                Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Failed));
                Assert.That(tokenVerificationResult.Message, Is.EqualTo("Token has expired."));
            });
        }

        [Test]
        public void TestVerify_WhenTokenNotExpiredAndExtraVerificationReturnedSuccess_ThenSuccessIsReturned()
        {
            DateTime expirationDate = DateTime.Now.AddMinutes(1);
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(
                this.GetTokenValue(expirationDate: expirationDate));
            var tokenId = new TokenId(encryptedTokenValue);
            var token = new TokenStub(tokenId);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Success));
        }
    }
}