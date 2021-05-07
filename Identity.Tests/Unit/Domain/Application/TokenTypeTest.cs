using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class TokenTypeTest
    {
        [Test]
        public void TestConstructing_WhenDefaultConstructorUsed_ThenNameIsSetToAccess()
        {
            var token = new TokenType();

            Assert.That(token.Name, Is.EqualTo("Access"));
        }

        [Test]
        public void TestAccess_WhenAccessTypeUsed_ThenNameIsSetToAccess()
        {
            var token = TokenType.Access;

            Assert.That(token.Name, Is.EqualTo("Access"));
        }

        [Test]
        public void TestRefresh_WhenRefreshTypeUsed_ThenNameIsSetToRefresh()
        {
            var token = TokenType.Refresh;

            Assert.That(token.Name, Is.EqualTo("Refresh"));
        }

        [Test]
        public void TestGenerateExpirationDate_WhenAccessTokenGiven_ThenTokenExpiresOneDayAfterGeneration()
        {
            var token = TokenType.Access;

            Assert.That(token.GenerateExpirationDate(), Is.EqualTo(DateTime.Now.AddDays(1)).Within(1).Hours);
        }

        [Test]
        public void TestGenerateExpirationDate_WhenRefreshTokenGiven_ThenTokenExpiresOneYearAfterGeneration()
        {
            var token = TokenType.Refresh;

            Assert.That(token.GenerateExpirationDate(), Is.EqualTo(DateTime.Now.AddYears(1)).Within(1).Hours);
        }

        [Test]
        public void TestFromName_WhenAccesshNameGiven_ThenRefreshTokenIsReturned()
        {
            TokenType token = TokenType.FromName("Access");

            Assert.That(token, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestFromName_WhenRefreshNameGiven_ThenRefreshTokenIsReturned()
        {
            TokenType token = TokenType.FromName("Refresh");

            Assert.That(token, Is.EqualTo(TokenType.Refresh));
        }

        [TestCase("")]
        [TestCase("asfasf")]
        [TestCase((string)null)]
        public void TestFromString_WhenUnknownTokenNameGiven_ThenInvalidTokenExceptionIsThrown(string name)
        {
            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token type given."),
                () => TokenType.FromName(name));
        }
    }
}