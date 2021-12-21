using Identity.Core.Domain;
using Identity.Core.Events;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class ApplicationCreatedTest
    {
        [Test]
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            ApplicationCreated applicationCreated = this.GetApplicationCreated(applicationId);

            Assert.That(applicationCreated.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
        }

        private ApplicationCreated GetApplicationCreated(
            ApplicationId applicationId = null, 
            string applicationName = null, 
            UserId applicationUserId = null,
            Url applicationHomepageUrl = null,
            Url applicationCallbackUrl = null)
        {
            return new ApplicationCreated(
                applicationId: applicationId ?? ApplicationId.Generate(),
                applicationName: applicationName ?? "MyApp",
                applicationUserId: applicationUserId ?? UserId.Generate(),
                applicationHomepageUrl: applicationHomepageUrl ?? new Url("http://wwww.example.com"),
                applicationCallbackUrl: applicationCallbackUrl ?? new Url("http://wwww.example.com/1"));
        }

        [Test]
        public void TestConstructor_WhenApplicationNameGiven_ThenApplicationNameIsSet()
        {
            ApplicationCreated applicationCreated = this.GetApplicationCreated(applicationName: "MyApp");

            Assert.That(applicationCreated.ApplicationName, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstructor_WhenApplicationUserIdGiven_ThenApplicationUserIdIsSet()
        {
            UserId userId = UserId.Generate();
            ApplicationCreated applicationCreated = this.GetApplicationCreated(applicationUserId: userId);

            Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenApplicationHomepageUrlGiven_ThenApplicationHomepageUrlIsSet()
        {
            ApplicationCreated applicationCreated = this.GetApplicationCreated(
                applicationHomepageUrl: new Url("http://wwww.example.com"));

            Assert.That(applicationCreated.ApplicationHomepageUrl, Is.EqualTo("http://wwww.example.com"));
        }

        [Test]
        public void TestConstructor_WhenApplicationCallbackUrlGiven_ThenApplicationCallbackUrlIsSet()
        {
            ApplicationCreated applicationCreated = this.GetApplicationCreated(
                applicationCallbackUrl: new Url("http://wwww.example.com/1"));

            Assert.That(applicationCreated.ApplicationCallbackUrl, Is.EqualTo("http://wwww.example.com/1"));
        }
    }
}