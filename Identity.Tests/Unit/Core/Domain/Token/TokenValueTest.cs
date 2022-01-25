using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class TokenValueTest
    {
        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenValueIsSet()
        {
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();

            TokenValue token = new(tokenValue);

            Assert.That(token.ToString(), Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithApplicationId(applicationId)
                .Build();
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();

            TokenValue token = new(tokenValue);

            Assert.That(token.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenTypeIsSet()
        {
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithType(TokenType.Refresh)
                .Build();
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();


            TokenValue token = new(tokenValue);

            Assert.That(token.Type, Is.EqualTo(TokenType.Refresh));
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenExpiresAtIsSet()
        {
            DateTime expirationDate = DateTime.Now;
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithExpirationDate(expirationDate)
                .Build();
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();

            TokenValue token = new(tokenValue);

            Assert.That(token.ExpiresAt, Is.EqualTo(expirationDate).Within(1).Seconds);
        }

        [Test]
        public void TestConstructor_WhenTokenValueGiven_ThenPermissionsAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource2"), "Add"),
                new PermissionId(new ResourceId("MyResource2"), "Remove")
            };
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithPermissions(permissions)
                .Build();
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();

            TokenValue token = new(tokenValue);

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
            PermissionId[] permissions = new PermissionId[]
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
            PermissionId[] permissions = new PermissionId[]
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
            PermissionId[] permissions = new PermissionId[]
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
            PermissionId[] permissions = new PermissionId[]
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
            PermissionId[] permissions = new PermissionId[]
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
            PermissionId[] permissions = new PermissionId[]
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
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(-1))
                .Build();
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();
            TokenValue token = new(tokenValue);

            bool expired = token.Expired;

            Assert.That(expired, Is.True);
        }

        [Test]
        public void TestExpired_WhenValidTokenGiven_ThenFalseIsReturned()
        {
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            string tokenValue = TokenValueEncoder.Encode(tokenInformation).ToString();
            TokenValue token = new(tokenValue);

            bool expired = token.Expired;

            Assert.That(expired, Is.False);
        }

        [Test]
        public void TestEquals_WhenSameTokenGiven_ThenTrueIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            TokenValue token = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(),
                permissions);
            TokenValue leftToken = new TokenValue(token.ToString());
            TokenValue rightToken = new TokenValue(token.ToString());

            Assert.That(leftToken.Equals(rightToken), Is.True);
        }

        [Test]
        public void TestEquals_WhenDifferentTokenGiven_ThenFalseIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            TokenValue leftToken = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(),
                permissions);
            TokenValue rightToken = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(),
                permissions);

            Assert.That(leftToken.Equals(rightToken), Is.False);
        }
    }
}