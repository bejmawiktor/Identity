using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
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

            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithId(id)
                .Build();

            Assert.That(tokenInformation.Id, Is.EqualTo(id));
        }

        [Test]
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();

            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithApplicationId(applicationId)
                .Build();

            Assert.That(tokenInformation.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenTokenTypeGiven_ThenTokenTypeIsSet()
        {
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithType(TokenType.Access)
                .Build();

            Assert.That(tokenInformation.TokenType, Is.EqualTo(TokenType.Access));
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithPermissions(permissions)
                .Build();

            Assert.That(tokenInformation.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenExpirationDateGiven_ThenExpirationDateIsSet()
        {
            DateTime expirationDate = DateTime.Now;

            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithExpirationDate(expirationDate)
                .Build();

            Assert.That(tokenInformation.ExpirationDate, Is.EqualTo(expirationDate));
        }

        [Test]
        public void TestConstructor_WhenExpirationDateNotGiven_ThenExpirationDateIsGeneratedFromTokenType()
        {
            DateTime expirationDate = DateTime.Now;
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithExpirationDate(null)
                .WithType(TokenType.Refresh)
                .Build();

            Assert.That(
                tokenInformation.ExpirationDate,
                Is.EqualTo(expirationDate.AddYears(1)).Within(1).Seconds);
        }
    }
}