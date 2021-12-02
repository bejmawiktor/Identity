using Identity.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class TokenValueTest
    {
        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenValueIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            string tokenValue = this.GetTokenValue(applicationId: applicationId);

            var token = new TokenValue(tokenValue);

            Assert.That(token.ToString(), Is.EqualTo(tokenValue));
        }

        private string GetTokenValue(
            Guid? id = null,
            ApplicationId applicationId = null, 
            TokenType tokenType = null, 
            IEnumerable<PermissionId> permissions = null,
            DateTime? expirationDate = null)
        {
            var tokenInformation = new TokenInformation(
                id ?? Guid.NewGuid(),
                applicationId ?? ApplicationId.Generate(), 
                tokenType ?? TokenType.Access,
                permissions ?? new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add"),
                    new PermissionId(new ResourceId("MyResource"), "Remove")
                },
                expirationDate);

            return TokenValueEncoder.Encode(tokenInformation).ToString();
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            string tokenValue = this.GetTokenValue(applicationId: applicationId);

            var token = new TokenValue(tokenValue);

            Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenTypeIsSet()
        {
            string tokenValue = this.GetTokenValue(tokenType: TokenType.Refresh);

            var token = new TokenValue(tokenValue);

            Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenExpiresAtIsSet()
        {
            DateTime expirationDate = DateTime.Now;
            string tokenValue = this.GetTokenValue(expirationDate: expirationDate);

            var token = new TokenValue(tokenValue);

            Assert.That(token.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenPermissionsAreSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            string tokenValue = this.GetTokenValue(permissions: permissions);

            var token = new TokenValue(tokenValue);

            Assert.That(token.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new TokenValue(null));
        }

        [Test]
        public void TestConstructor_WhenEmptyValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Given token value can't be empty."),
                () => new TokenValue(string.Empty));
        }

        [TestCase("a")]
        [TestCase("asfdgsg")]
        [TestCase("agggggg")]
        public void TestConstructor_WhenInvalidValueGiven_ThenInvalidTokenExceptionIsThrown(string token)
        {
            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>(),
                () => new TokenValue(token));
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

            TokenValue token = TokenValue.GenerateAccessToken(applicationId, permissions);

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
                () => TokenValue.GenerateAccessToken(null, permissions));
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
                () => TokenValue.GenerateAccessToken(ApplicationId.Generate(), null));
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

            TokenValue token = TokenValue.GenerateRefreshToken(applicationId, permissions, expiresAt);

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
                () => TokenValue.GenerateRefreshToken(null, permissions, expiresAt));
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
                () => TokenValue.GenerateRefreshToken(ApplicationId.Generate(), null, expiresAt));
        }

        [Test]
        public void TestExpired_WhenExpiredTokenGiven_ThenTrueIsReturned()
        {
            string tokenValue = this.GetTokenValue(
                tokenType: TokenType.Refresh,
                expirationDate: DateTime.Now.AddDays(-1));
            var token = new TokenValue(tokenValue);

            bool expired = token.Expired;

            Assert.That(expired, Is.True);
        }

        [Test]
        public void TestExpired_WhenValidTokenGiven_ThenFalseIsReturned()
        {
            string tokenValue = this.GetTokenValue(
                tokenType: TokenType.Refresh,
                expirationDate: DateTime.Now.AddDays(1));
            var token = new TokenValue(tokenValue);

            bool expired = token.Expired;

            Assert.That(expired, Is.False);
        }

        [Test]
        public void TestEquals_WhenSameTokenGiven_ThenTrueIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            var token = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(),
                permissions);
            var leftToken = new TokenValue(token.ToString());
            var rightToken = new TokenValue(token.ToString());

            Assert.That(leftToken.Equals(rightToken), Is.True);
        }

        [Test]
        public void TestEquals_WhenDifferentTokenGiven_ThenFalseIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            var leftToken = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(),
                permissions);
            var rightToken = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(),
                permissions);

            Assert.That(leftToken.Equals(rightToken), Is.False);
        }
    }
}