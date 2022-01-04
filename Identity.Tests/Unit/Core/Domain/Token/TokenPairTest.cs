using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class TokenPairTest
    {
        [Test]
        public void TestConstructor_WhenAccessTokenGiven_ThenAccessTokenIsSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue accessToken = TokenValue.GenerateAccessToken(applicationId, permissions);
            TokenValue refreshToken = TokenValue.GenerateRefreshToken(applicationId, permissions);

            TokenPair tokenPair = new(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        public void TestConstructor_WhenRefreshTokenGiven_ThenRefreshTokenIsSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue accessToken = TokenValue.GenerateAccessToken(applicationId, permissions);
            TokenValue refreshToken = TokenValue.GenerateRefreshToken(applicationId, permissions);

            TokenPair tokenPair = new(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        public void TestConstructor_WhenRefreshTokenGivenInAccessTokenPlace_ThenArgumentExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue refreshToken = TokenValue.GenerateRefreshToken(applicationId, permissions);

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong token type. Expected access token but got refresh instead."),
                () => new TokenPair(
                    accessToken: refreshToken,
                    refreshToken: refreshToken));
        }

        [Test]
        public void TestConstructor_WhenAccessTokenGivenInRefreshTokenPlace_ThenArgumentExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue accessToken = TokenValue.GenerateAccessToken(applicationId, permissions);

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong token type. Expected refresh token but got access instead."),
                () => new TokenPair(
                    accessToken: accessToken,
                    refreshToken: accessToken));
        }

        [Test]
        public void TestConstructor_WhenNullAccessTokenGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue refreshToken = TokenValue.GenerateRefreshToken(applicationId, permissions);

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("accessToken"),
               () => new TokenPair(
                    accessToken: null,
                    refreshToken: refreshToken));
        }

        [Test]
        public void TestConstructor_WhenNullRefreshTokenGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue accessToken = TokenValue.GenerateAccessToken(applicationId, permissions);

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("refreshToken"),
               () => new TokenPair(
                    accessToken: accessToken,
                    refreshToken: null));
        }
    }
}