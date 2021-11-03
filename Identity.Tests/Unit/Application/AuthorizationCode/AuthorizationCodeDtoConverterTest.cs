using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class AuthorizationCodeDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenAuthorizationCodeGiven_ThenAuthorizationCodeDtoIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCode = new AuthorizationCode(
                id: authorizationCodeId,
                expiresAt: expiresAt,
                used: true,
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource1"), "Add"),
                    new PermissionId(new ResourceId("MyResource2"), "Add")
                });
            var authorizationCodeDtoConverter = new AuthorizationCodeDtoConverter();

            AuthorizationCodeDto authorizationCodeDto = authorizationCodeDtoConverter.ToDto(authorizationCode);

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCodeDto.ApplicationId, Is.EqualTo(authorizationCodeId.ApplicationId.ToGuid()));
                Assert.That(authorizationCodeDto.Code, Is.EqualTo(authorizationCodeId.Code.ToString()));
                Assert.That(authorizationCodeDto.ExpiresAt, Is.EqualTo(expiresAt));
                Assert.That(authorizationCodeDto.Used, Is.True);
                Assert.That(authorizationCodeDto.Permissions, Is.EquivalentTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource1", "Add"),
                    ("MyResource2", "Add")
                }));
            });
        }

        [Test]
        public void TestToDto_WhenNullAuthorizationCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            var authorizationCodeDtoConverter = new AuthorizationCodeDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("authorizationCode"),
                () => authorizationCodeDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenAuthorizationCodeIdGiven_ThenDtoIdentifierIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDtoConverter = new AuthorizationCodeDtoConverter();

            (Guid ApplicationId, string Code) authorizationCodeDtoId = authorizationCodeDtoConverter.ToDtoIdentifier(authorizationCodeId);

            Assert.That(authorizationCodeDtoId, Is.EqualTo((authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code.ToString())));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullAuthorizationCodeIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var authorizationCodeDtoConverter = new AuthorizationCodeDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("authorizationCodeId"),
                () => authorizationCodeDtoConverter.ToDtoIdentifier(null));
        }
    }
}
