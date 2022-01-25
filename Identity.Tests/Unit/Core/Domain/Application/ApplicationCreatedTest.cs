using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
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
            ApplicationCreated applicationCreated = new ApplicationCreatedBuilder()
                .WithApplicationId(applicationId)
                .Build();

            Assert.That(applicationCreated.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenApplicationNameGiven_ThenApplicationNameIsSet()
        {
            ApplicationCreated applicationCreated = new ApplicationCreatedBuilder()
                .WithApplicationName("MyApp")
                .Build();

            Assert.That(applicationCreated.ApplicationName, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstructor_WhenApplicationUserIdGiven_ThenApplicationUserIdIsSet()
        {
            UserId userId = UserId.Generate();
            ApplicationCreated applicationCreated = new ApplicationCreatedBuilder()
                .WithApplicationUserId(applicationUserId: userId)
                .Build();

            Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenApplicationHomepageUrlGiven_ThenApplicationHomepageUrlIsSet()
        {
            ApplicationCreated applicationCreated = new ApplicationCreatedBuilder()
                .WithApplicationHomepageUrl(applicationHomepageUrl: new Url("http://wwww.example1.com"))
                .Build();

            Assert.That(applicationCreated.ApplicationHomepageUrl, Is.EqualTo("http://wwww.example1.com"));
        }

        [Test]
        public void TestConstructor_WhenApplicationCallbackUrlGiven_ThenApplicationCallbackUrlIsSet()
        {
            ApplicationCreated applicationCreated = new ApplicationCreatedBuilder()
                .WithApplicationCallbackUrl(applicationCallbackUrl: new Url("http://wwww.example.com/2"))
                .Build();

            Assert.That(applicationCreated.ApplicationCallbackUrl, Is.EqualTo("http://wwww.example.com/2"));
        }
    }
}