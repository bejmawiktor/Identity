using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class TokenInformationTest
    {
        [Test]
        public void TestConstructing_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            var userId = UserId.Generate();
            var applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access);

            Assert.That(tokenInformation.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructing_WhenTokenTypeGiven_ThenTokenTypeIsSet()
        {
            var userId = UserId.Generate();
            var applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access);

            Assert.That(tokenInformation.TokenType, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestConstructing_WhenExpirationDateGiven_ThenExpirationDateIsSet()
        {
            var userId = UserId.Generate();
            var applicationId = ApplicationId.Generate();
            var now = DateTime.Now;
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access,
                expirationDate: now);

            Assert.That(tokenInformation.ExpirationDate, Is.EqualTo(now));
        }

        [Test]
        public void TestConstructing_WhenExpirationDateNotGiven_ThenExpirationDateIsGeneratedFromTokenType()
        {
            var userId = UserId.Generate();
            var applicationId = ApplicationId.Generate();
            var now = DateTime.Now;
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Refresh,
                expirationDate: now);

            Assert.That(
                tokenInformation.ExpirationDate,
                Is.EqualTo(now));
        }
    }
}