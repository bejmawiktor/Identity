using Identity.Application;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using Application = Identity.Persistence.MSSQL.DataModels.Application;

    public class ApplicationTest
    {
        [Test]
        public void TestConstructing_WhenDtoGiven_ThenMembersAreSet()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var application = new Application(
                new ApplicationDto(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
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
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var application = new Application(
                new ApplicationDto(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
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