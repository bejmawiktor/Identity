using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class TokenTypeTest
    {
        [Test]
        public void TestConstructor_WhenDefaultConstructorUsed_ThenNameIsSetToAccess()
        {
            TokenType tokenType = new TokenType();

            Assert.That(tokenType.Name, Is.EqualTo("Access"));
        }

        [Test]
        public void TestAccess_WhenAccessTypeUsed_ThenNameIsSetToAccess()
        {
            TokenType tokenType = TokenType.Access;

            Assert.That(tokenType.Name, Is.EqualTo("Access"));
        }

        [Test]
        public void TestRefresh_WhenRefreshTypeUsed_ThenNameIsSetToRefresh()
        {
            TokenType tokenType = TokenType.Refresh;

            Assert.That(tokenType.Name, Is.EqualTo("Refresh"));
        }

        [Test]
        public void TestGenerateExpirationDate_WhenAccessTokenGiven_ThenTokenExpiresOneDayAfterGeneration()
        {
            TokenType tokenType = TokenType.Access;

            Assert.That(tokenType.GenerateExpirationDate(), Is.EqualTo(DateTime.Now.AddDays(1)).Within(1).Minutes);
        }

        [Test]
        public void TestGenerateExpirationDate_WhenRefreshTokenGiven_ThenTokenExpiresOneYearAfterGeneration()
        {
            TokenType tokenType = TokenType.Refresh;

            Assert.That(tokenType.GenerateExpirationDate(), Is.EqualTo(DateTime.Now.AddYears(1)).Within(1).Minutes);
        }

        [Test]
        public void TestFromName_WhenAccesshNameGiven_ThenRefreshTokenIsReturned()
        {
            TokenType tokenType = TokenType.FromName("Access");

            Assert.That(tokenType, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestFromName_WhenRefreshNameGiven_ThenRefreshTokenIsReturned()
        {
            TokenType tokenType = TokenType.FromName("Refresh");

            Assert.That(tokenType, Is.EqualTo(TokenType.Refresh));
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