using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class TokenPairTest
    {
        [Test]
        public void TestConstruction_WhenAccessTokenGiven_ThenAccessTokenIsSet()
        {
            var userId = UserId.Generate();
            var accessToken = Token.GenerateAccessToken(userId);
            var refreshToken = Token.GenerateRefreshToken(userId);
            var tokenPair = new TokenPair(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        public void TestConstruction_WhenRefreshTokenGiven_ThenRefreshTokenIsSet()
        {
            var userId = UserId.Generate();
            var accessToken = Token.GenerateAccessToken(userId);
            var refreshToken = Token.GenerateRefreshToken(userId);
            var tokenPair = new TokenPair(
                accessToken: accessToken,
                refreshToken: refreshToken);

            Assert.That(tokenPair.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        public void TestConstruction_WhenRefreshTokenGivenInAccessTokenPlace_ThenArgumentExceptionIsThrown()
        {
            var userId = UserId.Generate();
            var refreshToken = Token.GenerateRefreshToken(userId);

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
            var userId = UserId.Generate();
            var accessToken = Token.GenerateAccessToken(userId);

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
            var userId = UserId.Generate();
            var refreshToken = Token.GenerateRefreshToken(userId);

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
            var userId = UserId.Generate();
            var accessToken = Token.GenerateAccessToken(userId);

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