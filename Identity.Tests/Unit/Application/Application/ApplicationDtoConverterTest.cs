﻿using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application.Application
{
    using Application = Identity.Domain.Application;
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenApplicationGiven_ThenApplicationDtoIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));
            var applicationDtoConverter = new ApplicationDtoConverter();

            var applicationDto = applicationDtoConverter.ToDto(application);

            Assert.Multiple(() =>
            {
                Assert.That(applicationDto.Id, Is.EqualTo(applicationId.ToGuid()));
                Assert.That(applicationDto.UserId, Is.EqualTo(userId.ToGuid()));
                Assert.That(applicationDto.Name, Is.EqualTo("MyApp"));
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