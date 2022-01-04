using Identity.Core.Application;
using Identity.Core.Domain;
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

            ApplicationDto applicationDto = this.GetApplicationDto(applicationId);

            Assert.That(applicationDto.Id, Is.EqualTo(applicationId));
        }

        private ApplicationDto GetApplicationDto(
            Guid? id = null,
            Guid? userId = null,
            string name = null,
            string secretKey = null,
            string homepageUrl = null,
            string callbackUrl = null)
        {
            return new ApplicationDto(
                id: id ?? Guid.NewGuid(),
                userId: userId ?? Guid.NewGuid(),
                name: name ?? "MyApp",
                secretKey: secretKey ?? SecretKey,
                homepageUrl: homepageUrl ?? "http://www.example.com",
                callbackUrl: callbackUrl ?? "http://www.example.com/1");
        }

        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            Guid userId = Guid.NewGuid();
            ApplicationDto applicationDto = this.GetApplicationDto(userId: userId);

            Assert.That(applicationDto.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            ApplicationDto applicationDto = this.GetApplicationDto(name: "MyApp");

            Assert.That(applicationDto.Name, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstructor_WhenSecretKeyGiven_ThenSecretKeyIsSet()
        {
            ApplicationDto applicationDto = this.GetApplicationDto(secretKey: SecretKey);

            Assert.That(applicationDto.SecretKey, Is.EqualTo(SecretKey));
        }

        [Test]
        public void TestConstructor_WhenHomepageUrlGiven_ThenHomepageUrlIsSet()
        {
            ApplicationDto applicationDto = this.GetApplicationDto(homepageUrl: "http://www.example.com");

            Assert.That(applicationDto.HomepageUrl, Is.EqualTo("http://www.example.com"));
        }

        [Test]
        public void TestConstructor_WhenCallbackUrlGiven_ThenCallbackUrlIsSet()
        {
            ApplicationDto applicationDto = this.GetApplicationDto(callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.CallbackUrl, Is.EqualTo("http://www.example.com/1"));
        }

        [Test]
        public void TestToApplication_WhenConvertingToApplication_ThenApplicationIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ApplicationDto applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Application application = applicationDto.ToApplication();

            Assert.Multiple(() =>
            {
                Assert.That(application.Id, Is.EqualTo(new ApplicationId(applicationId)));
                Assert.That(application.UserId, Is.EqualTo(new UserId(userId)));
                Assert.That(application.Name, Is.EqualTo("MyApp"));
                Assert.That(application.SecretKey, Is.EqualTo(new EncryptedSecretKey(SecretKey)));
                Assert.That(application.HomepageUrl, Is.EqualTo(new Url("http://www.example.com")));
                Assert.That(application.CallbackUrl, Is.EqualTo(new Url("http://www.example.com/1")));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalApplicationsDtosGiven_ThenTrueIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ApplicationDto leftApplicationDto = this.GetApplicationDto(applicationId, userId);
            ApplicationDto rightApplicationDto = this.GetApplicationDto(applicationId, userId);

            Assert.That(leftApplicationDto.Equals(rightApplicationDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentApplicationsDtosGiven_ThenFalseIsReturned()
        {
            Guid firstApplicationId = Guid.NewGuid();
            Guid firstUserId = Guid.NewGuid();
            Guid secondApplicationId = Guid.NewGuid();
            Guid secondUserId = Guid.NewGuid();
            ApplicationDto leftApplicationDto = this.GetApplicationDto(firstApplicationId, firstUserId);
            ApplicationDto rightApplicationDto = this.GetApplicationDto(secondApplicationId, secondUserId);

            Assert.That(leftApplicationDto.Equals(rightApplicationDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalApplicationsDtosGiven_ThenSameHashCodesIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ApplicationDto leftApplicationDto = this.GetApplicationDto(applicationId, userId);
            ApplicationDto rightApplicationDto = this.GetApplicationDto(applicationId, userId);

            Assert.That(leftApplicationDto.GetHashCode(), Is.EqualTo(rightApplicationDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentApplicationsDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            Guid firstApplicationId = Guid.NewGuid();
            Guid firstUserId = Guid.NewGuid();
            Guid secondApplicationId = Guid.NewGuid();
            Guid secondUserId = Guid.NewGuid();
            ApplicationDto leftApplicationDto = this.GetApplicationDto(firstApplicationId, firstUserId);
            ApplicationDto rightApplicationDto = this.GetApplicationDto(secondApplicationId, secondUserId);

            Assert.That(leftApplicationDto.GetHashCode(), Is.Not.EqualTo(rightApplicationDto.GetHashCode()));
        }
    }
}