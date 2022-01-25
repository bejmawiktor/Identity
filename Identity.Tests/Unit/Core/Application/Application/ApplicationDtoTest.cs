using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationDtoTest
    {
        private static readonly string SecretKey = EncryptedSecretKey
            .Encrypt(Identity.Core.Domain.SecretKey.Generate())
            .ToString();

        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            Guid applicationId = Guid.NewGuid();

            ApplicationDto applicationDto = new ApplicationDtoBuilder()
                .WithId(applicationId)
                .Build();

            Assert.That(applicationDto.Id, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            Guid userId = Guid.NewGuid();

            ApplicationDto applicationDto = new ApplicationDtoBuilder()
                .WithUserId(userId)
                .Build();

            Assert.That(applicationDto.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            ApplicationDto applicationDto = new ApplicationDtoBuilder()
                .WithName("MyApp")
                .Build();

            Assert.That(applicationDto.Name, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstructor_WhenSecretKeyGiven_ThenSecretKeyIsSet()
        {
            ApplicationDto applicationDto = new ApplicationDtoBuilder()
                .WithSecretKey(SecretKey)
                .Build();

            Assert.That(applicationDto.SecretKey, Is.EqualTo(SecretKey));
        }

        [Test]
        public void TestConstructor_WhenHomepageUrlGiven_ThenHomepageUrlIsSet()
        {
            ApplicationDto applicationDto = new ApplicationDtoBuilder()
                .WithHompageUrl("http://www.example.com")
                .Build();

            Assert.That(applicationDto.HomepageUrl, Is.EqualTo("http://www.example.com"));
        }

        [Test]
        public void TestConstructor_WhenCallbackUrlGiven_ThenCallbackUrlIsSet()
        {
            ApplicationDto applicationDto = new ApplicationDtoBuilder()
                .WithCallbackUrl("http://www.example.com/1")
                .Build();

            Assert.That(applicationDto.CallbackUrl, Is.EqualTo("http://www.example.com/1"));
        }

        [Test]
        public void TestToApplication_WhenConvertingToApplication_ThenApplicationIsReturned()
        {
            ApplicationDto applicationDto = ApplicationDtoBuilder.DefaultApplicationDto;

            Application application = applicationDto.ToApplication();

            Assert.Multiple(() =>
            {
                Assert.That(application.Id, Is.EqualTo(new ApplicationId(applicationDto.Id)));
                Assert.That(application.UserId, Is.EqualTo(new UserId(applicationDto.UserId)));
                Assert.That(application.Name, Is.EqualTo(applicationDto.Name));
                Assert.That(application.SecretKey, Is.EqualTo(new EncryptedSecretKey(applicationDto.SecretKey)));
                Assert.That(application.HomepageUrl, Is.EqualTo(new Url(applicationDto.HomepageUrl)));
                Assert.That(application.CallbackUrl, Is.EqualTo(new Url(applicationDto.CallbackUrl)));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalApplicationsDtosGiven_ThenTrueIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ApplicationDtoBuilder applicationDtoBuilder = new ApplicationDtoBuilder()
                .WithId(applicationId)
                .WithUserId(userId);
            ApplicationDto leftApplicationDto = applicationDtoBuilder.Build();
            ApplicationDto rightApplicationDto = applicationDtoBuilder.Build();

            Assert.That(leftApplicationDto.Equals(rightApplicationDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentApplicationsDtosGiven_ThenFalseIsReturned()
        {
            Guid firstApplicationId = Guid.NewGuid();
            Guid firstUserId = Guid.NewGuid();
            Guid secondApplicationId = Guid.NewGuid();
            Guid secondUserId = Guid.NewGuid();
            ApplicationDto leftApplicationDto = new ApplicationDtoBuilder()
                .WithId(firstApplicationId)
                .WithUserId(firstUserId)
                .Build();
            ApplicationDto rightApplicationDto = new ApplicationDtoBuilder()
                .WithId(secondApplicationId)
                .WithUserId(secondUserId)
                .Build();

            Assert.That(leftApplicationDto.Equals(rightApplicationDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalApplicationsDtosGiven_ThenSameHashCodesIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ApplicationDtoBuilder applicationDtoBuilder = new ApplicationDtoBuilder()
                .WithId(applicationId)
                .WithUserId(userId);
            ApplicationDto leftApplicationDto = applicationDtoBuilder.Build();
            ApplicationDto rightApplicationDto = applicationDtoBuilder.Build();

            Assert.That(leftApplicationDto.GetHashCode(), Is.EqualTo(rightApplicationDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentApplicationsDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            Guid firstApplicationId = Guid.NewGuid();
            Guid firstUserId = Guid.NewGuid();
            Guid secondApplicationId = Guid.NewGuid();
            Guid secondUserId = Guid.NewGuid();
            ApplicationDto leftApplicationDto = new ApplicationDtoBuilder()
                .WithId(firstApplicationId)
                .WithUserId(firstUserId)
                .Build();
            ApplicationDto rightApplicationDto = new ApplicationDtoBuilder()
                .WithId(secondApplicationId)
                .WithUserId(secondUserId)
                .Build();

            Assert.That(leftApplicationDto.GetHashCode(), Is.Not.EqualTo(rightApplicationDto.GetHashCode()));
        }
    }
}