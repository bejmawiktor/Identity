using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    using Application = Identity.Domain.Application;
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationDtoTest
    {
        private static readonly string SecretKey = EncryptedSecretKey
            .Encrypt(Identity.Domain.SecretKey.Generate())
            .ToString();

        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.Id, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructing_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructing_WhenNameGiven_ThenNameIsSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.Name, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstructing_WhenSecretKeyGiven_ThenSecretKeyIsSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.SecretKey, Is.EqualTo(SecretKey));
        }

        [Test]
        public void TestConstructing_WhenHomepageUrlGiven_ThenHomepageUrlIsSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.HomepageUrl, Is.EqualTo("http://www.example.com"));
        }

        [Test]
        public void TestConstructing_WhenCallbackUrlGiven_ThenCallbackUrlIsSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(applicationDto.CallbackUrl, Is.EqualTo("http://www.example.com/1"));
        }

        [Test]
        public void TestToApplication_WhenConvertingToApplication_ThenApplicationIsReturned()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
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
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var leftApplicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");
            var rightApplicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(leftApplicationDto.Equals(rightApplicationDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentApplicationsDtosGiven_ThenFalseIsReturned()
        {
            var firstApplicationId = Guid.NewGuid();
            var firstUserId = Guid.NewGuid();
            var secondApplicationId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            var leftApplicationDto = new ApplicationDto(
                id: firstApplicationId,
                userId: firstUserId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");
            var rightApplicationDto = new ApplicationDto(
                id: secondApplicationId,
                userId: secondUserId,
                name: "MyApp2",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(leftApplicationDto.Equals(rightApplicationDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalApplicationsDtosGiven_ThenSameHashCodesIsReturned()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var leftApplicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");
            var rightApplicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(leftApplicationDto.GetHashCode(), Is.EqualTo(rightApplicationDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentApplicationsDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            var firstApplicationId = Guid.NewGuid();
            var firstUserId = Guid.NewGuid();
            var secondApplicationId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            var leftApplicationDto = new ApplicationDto(
                id: firstApplicationId,
                userId: firstUserId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");
            var rightApplicationDto = new ApplicationDto(
                id: secondApplicationId,
                userId: secondUserId,
                name: "MyApp2",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1");

            Assert.That(leftApplicationDto.GetHashCode(), Is.Not.EqualTo(rightApplicationDto.GetHashCode()));
        }
    }
}