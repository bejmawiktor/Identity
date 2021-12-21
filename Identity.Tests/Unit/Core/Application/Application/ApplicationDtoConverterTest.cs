using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationDtoConverterTest
    {
        private static readonly EncryptedSecretKey EncryptedSecretKey
            = EncryptedSecretKey.Encrypt(Identity.Core.Domain.SecretKey.Generate());

        [Test]
        public void TestToDto_WhenApplicationGiven_ThenApplicationDtoIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: EncryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));
            var applicationDtoConverter = new ApplicationDtoConverter();

            ApplicationDto applicationDto = applicationDtoConverter.ToDto(application);

            Assert.Multiple(() =>
            {
                Assert.That(applicationDto.Id, Is.EqualTo(applicationId.ToGuid()));
                Assert.That(applicationDto.UserId, Is.EqualTo(userId.ToGuid()));
                Assert.That(applicationDto.Name, Is.EqualTo("MyApp"));
                Assert.That(applicationDto.SecretKey, Is.EqualTo(EncryptedSecretKey.ToString()));
                Assert.That(applicationDto.HomepageUrl, Is.EqualTo("https://www.example.com"));
                Assert.That(applicationDto.CallbackUrl, Is.EqualTo("https://www.example.com/1"));
            });
        }

        [Test]
        public void TestToDto_WhenNullApplicationGiven_ThenArgumentNullExceptionIsThrown()
        {
            var applicationDtoConverter = new ApplicationDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("application"),
                () => applicationDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenApplicationIdGiven_ThenDtoIdentifierIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var applicationDtoConverter = new ApplicationDtoConverter();

            Guid applicationDtoId = applicationDtoConverter.ToDtoIdentifier(applicationId);

            Assert.That(applicationDtoId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var applicationDtoConverter = new ApplicationDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationId"),
                () => applicationDtoConverter.ToDtoIdentifier(null));
        }
    }
}