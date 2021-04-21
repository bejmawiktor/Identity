using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class ApplicationCreatedTest
    {
        [Test]
        public void TestConstructing_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            var userId = UserId.Generate();
            var applicationId = ApplicationId.Generate();
            var applicationCreated = new ApplicationCreated(
                applicationId: applicationId,
                applicationName: "MyApp",
                applicationUserId: userId,
                applicationHomepageUrl: new Url("http://wwww.example.com"),
                applicationCallbackUrl: new Url("http://wwww.example.com/1"));

            Assert.That(applicationCreated.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void TestConstructing_WhenApplicationNameGiven_ThenApplicationNameIsSet()
        {
            var userId = UserId.Generate();
            var applicationCreated = new ApplicationCreated(
                applicationId: ApplicationId.Generate(),
                applicationName: "MyApp",
                applicationUserId: userId,
                applicationHomepageUrl: new Url("http://wwww.example.com"),
                applicationCallbackUrl: new Url("http://wwww.example.com/1"));

            Assert.That(applicationCreated.ApplicationName, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstructing_WhenApplicationUserIdGiven_ThenApplicationUserIdIsSet()
        {
            var userId = UserId.Generate();
            var applicationCreated = new ApplicationCreated(
                applicationId: ApplicationId.Generate(),
                applicationName: "MyApp",
                applicationUserId: userId,
                applicationHomepageUrl: new Url("http://wwww.example.com"),
                applicationCallbackUrl: new Url("http://wwww.example.com/1"));

            Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructing_WhenApplicationHomepageUrlGiven_ThenApplicationHomepageUrlIsSet()
        {
            var userId = UserId.Generate();
            var applicationCreated = new ApplicationCreated(
                applicationId: ApplicationId.Generate(),
                applicationName: "MyApp",
                applicationUserId: userId,
                applicationHomepageUrl: new Url("http://wwww.example.com"),
                applicationCallbackUrl: new Url("http://wwww.example.com/1"));

            Assert.That(applicationCreated.ApplicationHomepageUrl, Is.EqualTo(new Url("http://wwww.example.com")));
        }

        [Test]
        public void TestConstructing_WhenApplicationCallbackUrlGiven_ThenApplicationCallbackUrlIsSet()
        {
            var userId = UserId.Generate();
            var applicationCreated = new ApplicationCreated(
                applicationId: ApplicationId.Generate(),
                applicationName: "MyApp",
                applicationUserId: userId,
                applicationHomepageUrl: new Url("http://wwww.example.com"),
                applicationCallbackUrl: new Url("http://wwww.example.com/1"));

            Assert.That(applicationCreated.ApplicationCallbackUrl, Is.EqualTo(new Url("http://wwww.example.com/1")));
        }
    }
}