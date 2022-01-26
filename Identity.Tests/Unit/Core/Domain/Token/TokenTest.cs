using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class TokenTest
    {
        [Test]
        public void TestToString_WhenTokenIdGiven_ThenValueIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue tokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            TokenStub token = new(tokenId);

            Assert.That(token.ToString(), Is.EqualTo(tokenId.ToString()));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue tokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            TokenStub token = new(tokenId);

            Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenTypeIsSet()
        {
            TokenValue tokenValue = new TokenValueBuilder()
                .WithType(TokenType.Refresh)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            TokenStub token = new(tokenId);

            Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenExpiresAtIsSet()
        {
            DateTime expirationDate = DateTime.Now;
            TokenValue tokenValue = new TokenValueBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);

            TokenStub token = new(tokenId);

            Assert.That(token.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenPermissionsAreSet()
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

            TokenStub token = new(tokenId);

            Assert.That(token.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestVerify_WhenExtraVerificationReturnedFailedAndTokenExpired_ThenTokenVerificationFailedIsReturnedWithExtraVerificationMessage()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(TokenValueBuilder.DefaultTokenValue);
            TokenId tokenId = new(encryptedTokenValue);
            TokenStub token = new(tokenId, true);

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
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(TokenValueBuilder.DefaultTokenValue);
            TokenId tokenId = new(encryptedTokenValue);
            TokenStub token = new(tokenId, true);

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
            TokenValue tokenValue = new TokenValueBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);
            TokenStub token = new(tokenId);

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
            TokenValue tokenValue = new TokenValueBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            TokenId tokenId = new(encryptedTokenValue);
            TokenStub token = new(tokenId);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Success));
        }
    }
}