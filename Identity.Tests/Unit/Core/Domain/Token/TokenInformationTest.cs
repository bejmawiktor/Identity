using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class TokenInformationTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenApplicationIdIsSet()
        {
            Guid id = Guid.NewGuid();

            TokenInformation tokenInformation = this.GetTokenInformation(id);

            Assert.That(tokenInformation.Id, Is.EqualTo(id));
        }

        [Test]
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            
            TokenInformation tokenInformation = this.GetTokenInformation(applicationId: applicationId);

            Assert.That(tokenInformation.ApplicationId, Is.EqualTo(applicationId));
        }

        private TokenInformation GetTokenInformation(
            Guid? id = null,
            ApplicationId applicationId = null,
            TokenType tokenType = null,
            PermissionId[] permissions = null,
            DateTime? expirationDate = null)
        {
            var permissionsReplacement = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            return new TokenInformation(
                id ?? Guid.NewGuid(),
                applicationId ?? ApplicationId.Generate(),
                tokenType ?? TokenType.Refresh,
                permissions ?? permissionsReplacement,
                expirationDate);
        }

        [Test]
        public void TestConstructor_WhenTokenTypeGiven_ThenTokenTypeIsSet()
        {
            TokenInformation tokenInformation = this.GetTokenInformation(tokenType: TokenType.Access);

            Assert.That(tokenInformation.TokenType, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            TokenInformation tokenInformation = this.GetTokenInformation(permissions: permissions);

            Assert.That(tokenInformation.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenExpirationDateGiven_ThenExpirationDateIsSet()
        {
            DateTime expirationDate = DateTime.Now;

            TokenInformation tokenInformation = this.GetTokenInformation(expirationDate: expirationDate);

            Assert.That(tokenInformation.ExpirationDate, Is.EqualTo(expirationDate));
        }

        [Test]
        public void TestConstructor_WhenExpirationDateNotGiven_ThenExpirationDateIsGeneratedFromTokenType()
        {
            DateTime expirationDate = DateTime.Now;
            TokenInformation tokenInformation = this.GetTokenInformation(
                tokenType: TokenType.Refresh,
                expirationDate: null);

            Assert.That(
                tokenInformation.ExpirationDate,
                Is.EqualTo(expirationDate.AddYears(1)).Within(1).Seconds);
        }
    }
}