using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class TokenTest
    {
        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenValueIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh, permissions);
            string tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.ToString(), Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh, permissions);
            string tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenTypeIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh, permissions);
            string tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenExpiresAtIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            DateTime expirationDate = DateTime.Now.AddDays(1);
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh, permissions, expirationDate);
            string tokenValue = Token.TokenGenerationAlgorithm.Encode(tokenInformation);

            var token = new Token(tokenValue);

            Assert.That(token.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new Token(null));
        }

        [Test]
        public void TestConstructor_WhenEmptyValueGiven_ThenArgumentExceptionIsThrown()
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
        public void TestConstructor_WhenInvalidValueGiven_ThenInvalidTokenExceptionIsThrown(string token)
        {
            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>(),
                () => new Token(token));
        }

        [Test]
        public void TestGenerateAccessToken_WhenGenerated_ThenTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Token token = Token.GenerateAccessToken(applicationId, permissions);

            Assert.Multiple(() =>
            {
                Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(token.Type, Is.EqualTo(TokenType.Access));
                Assert.That(token.ExpiresAt, Is.EqualTo(DateTime.Now.AddDays(1)).Within(1).Hours);
                Assert.That(token.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestGenerateAccessToken_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationId"),
                () => Token.GenerateAccessToken(null, permissions));
        }

        [Test]
        public void TestGenerateAccessToken_WhenNullPermissionsGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissions"),
                () => Token.GenerateAccessToken(ApplicationId.Generate(), null));
        }

        [Test]
        public void TestGenerateRefreshToken_WhenGenerated_ThenTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            DateTime expiresAt = DateTime.Now;
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Token token = Token.GenerateRefreshToken(applicationId, permissions, expiresAt);

            Assert.Multiple(() =>
            {
                Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
                Assert.That(token.ExpiresAt, Is.EqualTo(expiresAt).Within(1).Seconds);
                Assert.That(token.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestGenerateRefreshToken_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            DateTime expiresAt = DateTime.Now;
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationId"),
                () => Token.GenerateRefreshToken(null, permissions, expiresAt));
        }

        [Test]
        public void TestGenerateRefreshToken_WhenNullPermissionsdGiven_ThenArgumentNullExceptionIsThrown()
        {
            DateTime expiresAt = DateTime.Now;
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissions"),
                () => Token.GenerateRefreshToken(ApplicationId.Generate(), null, expiresAt));
        }

        [Test]
        public void TestVerify_WhenCorrectTokenGiven_ThenSuccessIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            Token token = Token.GenerateAccessToken(applicationId, permissions);

            TokenVerificationResult tokenVerificationResult = token.Verify();

            Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Success));
        }

        [Test]
        public void TestVerify_WhenExpiredTokenGiven_ThenFailedIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            string tokenString = Token.TokenGenerationAlgorithm.Encode(
                new TokenInformation(applicationId, TokenType.Refresh, permissions, DateTime.Now.AddDays(-1)));
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