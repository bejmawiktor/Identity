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
        public void TestConstruction_WhenAccessTokenGiven_ThenAccessTokenIsSet()
        {
            var applicationId = ApplicationId.Generate();
            var accessToken = Token.GenerateAccessToken(applicationId);
            var refreshToken = Token.GenerateRefreshToken(applicationId);
            var tokenPair = new TokenPair(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        public void TestConstruction_WhenRefreshTokenGiven_ThenRefreshTokenIsSet()
        {
            var applicationId = ApplicationId.Generate();
            var accessToken = Token.GenerateAccessToken(applicationId);
            var refreshToken = Token.GenerateRefreshToken(applicationId);
            var tokenPair = new TokenPair(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        public void TestConstruction_WhenRefreshTokenGivenInAccessTokenPlace_ThenArgumentExceptionIsThrown()
        {
            var applicationId = ApplicationId.Generate();
            var refreshToken = Token.GenerateRefreshToken(applicationId);

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong token type. Expected access token but got refresh instead."),
                () => new TokenPair(
                    accessToken: refreshToken,
                    refreshToken: refreshToken));
        }

        [Test]
        public void TestConstruction_WhenAccessTokenGivenInRefreshTokenPlace_ThenArgumentExceptionIsThrown()
        {
            var applicationId = ApplicationId.Generate();
            var accessToken = Token.GenerateAccessToken(applicationId);

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong token type. Expected refresh token but got access instead."),
                () => new TokenPair(
                    accessToken: accessToken,
                    refreshToken: accessToken));
        }

        [Test]
        public void TestConstruction_WhenNullAccessTokenGiven_ThenArgumentNullExceptionIsThrown()
        {
            var applicationId = ApplicationId.Generate();
            var refreshToken = Token.GenerateRefreshToken(applicationId);

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("accessToken"),
               () => new TokenPair(
                    accessToken: null,
                    refreshToken: refreshToken));
        }

        [Test]
        public void TestConstruction_WhenNullRefreshTokenGiven_ThenArgumentNullExceptionIsThrown()
        {
            var applicationId = ApplicationId.Generate();
            var accessToken = Token.GenerateAccessToken(applicationId);

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