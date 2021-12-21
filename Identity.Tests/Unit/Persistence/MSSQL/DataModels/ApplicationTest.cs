using Identity.Core.Application;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using Application = Identity.Persistence.MSSQL.DataModels.Application;

    public class ApplicationTest
    {
        private static readonly string SecretKey = Identity.Core.Domain.SecretKey.Generate().ToString();

        [Test]
        public void TestConstructor_WhenDtoGiven_ThenMembersAreSet()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var application = new Application(
                new ApplicationDto(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
                    secretKey: SecretKey,
                    homepageUrl: "http://www.example.com",
                    callbackUrl: "http://www.example.com/1"));

            Assert.Multiple(() =>
            {
                Assert.That(application.Id, Is.EqualTo(applicationId));
                Assert.That(application.UserId, Is.EqualTo(userId));
                Assert.That(application.Name, Is.EqualTo("MyApp"));
                Assert.That(application.HomepageUrl, Is.EqualTo("http://www.example.com"));
                Assert.That(application.CallbackUrl, Is.EqualTo("http://www.example.com/1"));
            });
        }

        [Test]
        public void TestSetFields_WhenDtoGiven_ThenMembersAreSet()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var application = new Application();

            application.SetFields(new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: SecretKey,
                homepageUrl: "http://www.example.com",
                callbackUrl: "http://www.example.com/1"));

            Assert.Multiple(() =>
            {
                Assert.That(application.Id, Is.EqualTo(applicationId));
                Assert.That(application.UserId, Is.EqualTo(userId));
                Assert.That(application.Name, Is.EqualTo("MyApp"));
                Assert.That(application.HomepageUrl, Is.EqualTo("http://www.example.com"));
                Assert.That(application.CallbackUrl, Is.EqualTo("http://www.example.com/1"));
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenApplicationDtoIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var application = new Application(
                new ApplicationDto(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
                    secretKey: SecretKey,
                    homepageUrl: "http://www.example.com",
                    callbackUrl: "http://www.example.com/1"));

            ApplicationDto applicationDto = application.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(applicationDto.Id, Is.EqualTo(applicationId));
                Assert.That(applicationDto.UserId, Is.EqualTo(userId));
                Assert.That(applicationDto.Name, Is.EqualTo("MyApp"));
                Assert.That(applicationDto.HomepageUrl, Is.EqualTo("http://www.example.com"));
                Assert.That(applicationDto.CallbackUrl, Is.EqualTo("http://www.example.com/1"));
            });
        }
    }
}