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
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access);

            Assert.That(tokenInformation.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenTypeGiven_ThenTokenTypeIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access);

            Assert.That(tokenInformation.TokenType, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestConstructor_WhenExpirationDateGiven_ThenExpirationDateIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            DateTime now = DateTime.Now;
            var tokenInformation = new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access,
                expirationDate: now);

            Assert.That(tokenInformation.ExpirationDate, Is.EqualTo(now));
        }

        [Test]
        public void TestConstructor_WhenExpirationDateNotGiven_ThenExpirationDateIsGeneratedFromTokenType()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            DateTime now = DateTime.Now;
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