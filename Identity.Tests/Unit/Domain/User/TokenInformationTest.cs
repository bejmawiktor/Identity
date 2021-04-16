using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class TokenInformationTest
    {
        [Test]
        public void TestConstructing_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var tokenInformation = new TokenInformation(
                userId: userId,
                tokenType: TokenType.Access);

            Assert.That(tokenInformation.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructing_WhenTokenTypeGiven_ThenTokenTypeIsSet()
        {
            var userId = UserId.Generate();
            var tokenInformation = new TokenInformation(
                userId: userId,
                tokenType: TokenType.Access);

            Assert.That(tokenInformation.TokenType, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestConstructing_WhenExpirationDateGiven_ThenExpirationDateIsSet()
        {
            var userId = UserId.Generate();
            var now = DateTime.Now;
            var tokenInformation = new TokenInformation(
                userId: userId,
                tokenType: TokenType.Access,
                expirationDate: now);

            Assert.That(tokenInformation.ExpirationDate, Is.EqualTo(now));
        }

        [Test]
        public void TestConstructing_WhenExpirationDateNotGiven_ThenExpirationDateIsGeneratedFromTokenType()
        {
            var userId = UserId.Generate();
            var now = DateTime.Now;
            var tokenInformation = new TokenInformation(
                userId: userId,
                tokenType: TokenType.Refresh,
                expirationDate: now);

            Assert.That(
                tokenInformation.ExpirationDate,
                Is.EqualTo(now));
        }
    }
}