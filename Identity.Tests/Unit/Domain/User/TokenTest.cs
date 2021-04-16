using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class TokenTest
    {
        [Test]
        public void TestConstruction_WhenTokenValueGiven_ThenValueIsSet()
        {
            UserId userId = UserId.Generate();
            var tokenInformation = new TokenInformation(userId, TokenType.Refresh);
            var tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.ToString(), Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestConstruction_WhenTokenValueGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();
            var tokenInformation = new TokenInformation(userId, TokenType.Refresh);
            var tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenTokenValueGiven_ThenTypeIsSet()
        {
            UserId userId = UserId.Generate();
            var tokenInformation = new TokenInformation(userId, TokenType.Refresh);
            var tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstruction_WhenTokenValueGiven_ThenExpiresAtIsSet()
        {
            UserId userId = UserId.Generate();
            var expirationDate = DateTime.Now.AddDays(1);
            var tokenInformation = new TokenInformation(userId, TokenType.Refresh, expirationDate);
            var tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstruction_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new Token(null));
        }

        [Test]
        public void TestConstruction_WhenEmptyValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Given token value can't be empty."),
                () => new Token(string.Empty));
        }

        [TestCase("a")]
        [TestCase("asfdgsg")]
        [TestCase("agggggg")]
        public void TestConstruction_WhenInvalidValueGiven_ThenInvalidTokenExceptionIsThrown(string token)
        {
            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>(),
                () => new Token(token));
        }

        [Test]
        public void TestGenerateAccessToken_WhenGenerated_ThenTokenIsReturned()
        {
            var userId = UserId.Generate();

            Token token = Token.GenerateAccessToken(userId);

            Assert.Multiple(() =>
            {
                Assert.That(token.UserId, Is.EqualTo(userId));
                Assert.That(token.Type, Is.EqualTo(TokenType.Access));
                Assert.That(token.ExpiresAt, Is.EqualTo(DateTime.Now.AddDays(1)).Within(1).Hours);
            });
        }

        [Test]
        public void TestGenerateAccessToken_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var userId = UserId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("userId"),
                () => Token.GenerateAccessToken(null));
        }

        [Test]
        public void TestGenerateRefreshToken_WhenGenerated_ThenTokenIsReturned()
        {
            var userId = UserId.Generate();
            var expiresAt = DateTime.Now;

            Token token = Token.GenerateRefreshToken(userId, expiresAt);

            Assert.Multiple(() =>
            {
                Assert.That(token.UserId, Is.EqualTo(userId));
                Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
                Assert.That(token.ExpiresAt, Is.EqualTo(expiresAt).Within(1).Seconds);
            });
        }

        [Test]
        public void TestGenerateRefreshToken_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var userId = UserId.Generate();
            var expiresAt = DateTime.Now;

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("userId"),
                () => Token.GenerateRefreshToken(null, expiresAt));
        }
        
        [Test]
        public void TestVerify_WhenCorrectTokenGiven_ThenSuccessIsReturned()
        {
            var userId = UserId.Generate();
            Token token = Token.GenerateAccessToken(userId);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Success));
        }

        [Test]
        public void TestVerify_WhenExpiredTokenGiven_ThenFailedIsReturned()
        {
            var userId = UserId.Generate();
            string tokenString = Token.TokenGenerationAlgorithm.Encode(
                new TokenInformation(userId, TokenType.Refresh, DateTime.Now.AddDays(-1)));
            var token = new Token(tokenString);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.Multiple(() =>
            {
                Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Failed));
                Assert.That(tokenVerificationResult.Message, Is.EqualTo("Token has expired."));
            });
        }
    }
}