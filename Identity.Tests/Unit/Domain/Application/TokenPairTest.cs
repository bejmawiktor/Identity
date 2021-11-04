using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class TokenPairTest
    {
        [Test]
        public void TestConstructor_WhenAccessTokenGiven_ThenAccessTokenIsSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            Token accessToken = Token.GenerateAccessToken(applicationId, permissions);
            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions);
            var tokenPair = new TokenPair(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        public void TestConstructor_WhenRefreshTokenGiven_ThenRefreshTokenIsSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            Token accessToken = Token.GenerateAccessToken(applicationId, permissions);
            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions);
            var tokenPair = new TokenPair(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        public void TestConstructor_WhenRefreshTokenGivenInAccessTokenPlace_ThenArgumentExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions);

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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            Token accessToken = Token.GenerateAccessToken(applicationId, permissions);

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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions);

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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            ApplicationId applicationId = ApplicationId.Generate();
            Token accessToken = Token.GenerateAccessToken(applicationId, permissions);

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