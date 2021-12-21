using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class RefreshTokenDtoTest
    {
        [Test]
        public void TestConstructor_WhenRefreshTokenIdGiven_ThenInvalidOperationExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Access token id given."),
                () => new RefreshToken(TokenId.GenerateAccessTokenId(ApplicationId.Generate(), permissions)));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            var refreshToken = new RefreshToken(
                TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions),
                true);

            Assert.That(refreshToken.Used, Is.True);
        }

        [Test]
        public void TestVerify_WhenUsedTokenGiven_ThenFailedIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var refreshToken = new RefreshToken(
                TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions),
                true);

            TokenVerificationResult verificationResult = refreshToken.Verify();

            Assert.Multiple(() =>
            {
                Assert.That(verificationResult, Is.EqualTo(TokenVerificationResult.Failed));
                Assert.That(verificationResult.Message, Is.EqualTo("Token was used before."));
            });
        }

        [Test]
        public void TestVerify_WhenUnusedTokenGiven_ThenSuccessIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var refreshToken = new RefreshToken(
                TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions));

            TokenVerificationResult verificationResult = refreshToken.Verify();

            Assert.That(verificationResult, Is.EqualTo(TokenVerificationResult.Success));
        }

        [Test]
        public void TestUse_WhenPreviouslyUsed_ThenInvalidOperationExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var refreshToken = new RefreshToken(
                TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions),
                true);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Token was used before."),
                () => refreshToken.Use());
        }

        [Test]
        public void TestUse_WhenExpired_ThenInvalidOperationExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var refreshToken = new RefreshToken(
                TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions, DateTime.Now.AddDays(-1)),
                false);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Token has expired."),
                () => refreshToken.Use());
        }

        [Test]
        public void TestUse_WhenUsing_ThenUsedIsSetToTrue()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var refreshToken = new RefreshToken(
                TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions));

            refreshToken.Use();

            Assert.That(refreshToken.Used, Is.True);
        }
    }
}