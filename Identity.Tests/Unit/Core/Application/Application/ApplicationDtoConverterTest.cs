using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenApplicationGiven_ThenApplicationDtoIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            Application application = ApplicationBuilder.DefaultApplication;
            ApplicationDtoConverter applicationDtoConverter = new();

            ApplicationDto applicationDto = applicationDtoConverter.ToDto(application);

            Assert.Multiple(() =>
            {
                Assert.That(applicationDto.Id, Is.EqualTo(application.Id.ToGuid()));
                Assert.That(applicationDto.UserId, Is.EqualTo(application.UserId.ToGuid()));
                Assert.That(applicationDto.Name, Is.EqualTo(application.Name));
                Assert.That(applicationDto.SecretKey, Is.EqualTo(application.SecretKey.ToString()));
                Assert.That(applicationDto.HomepageUrl, Is.EqualTo(application.HomepageUrl.ToString()));
                Assert.That(applicationDto.CallbackUrl, Is.EqualTo(application.CallbackUrl.ToString()));
            });
        }

        [Test]
        public void TestToDto_WhenNullApplicationGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationDtoConverter applicationDtoConverter = new();

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
            ApplicationDtoConverter applicationDtoConverter = new();

            Guid applicationDtoId = applicationDtoConverter.ToDtoIdentifier(applicationId);

            Assert.That(applicationDtoId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationDtoConverter applicationDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationId"),
                () => applicationDtoConverter.ToDtoIdentifier(null));
        }
    }
}