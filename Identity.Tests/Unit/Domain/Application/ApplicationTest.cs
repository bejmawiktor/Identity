using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using Application = Identity.Domain.Application;
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.That(application.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("userId"),
                () => new Application(
                    id: applicationId,
                    userId: null,
                    name: "MyApp",
                    homepageUrl: new Url("https://www.example.com"),
                    callbackUrl: new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstruction_WhenNameGiven_ThenNameIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.That(application.Name, Is.EqualTo("MyApp"));
        }

        [Test]
        public void TestConstruction_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("name"),
                () => new Application(
                    id: applicationId,
                    userId: userId,
                    name: null,
                    homepageUrl: new Url("https://www.example.com"),
                    callbackUrl: new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstruction_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Name can't be empty."),
                () => new Application(
                    id: applicationId,
                    userId: userId,
                    name: string.Empty,
                    homepageUrl: new Url("https://www.example.com"),
                    callbackUrl: new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstruction_WhenHomepageUrlGiven_ThenHomepageUrlIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.That(application.HomepageUrl, Is.EqualTo(new Url("https://www.example.com")));
        }

        [Test]
        public void TestConstruction_WhenNullHomepageUrlGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("homepageUrl"),
                () => new Application(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
                    homepageUrl: null,
                    callbackUrl: new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstruction_WhenCallbackUrlGiven_ThenCallbackUrlIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.That(application.CallbackUrl, Is.EqualTo(new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstruction_WhenNullCallbackUrlGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("callbackUrl"),
                () => new Application(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
                    homepageUrl: new Url("https://www.example.com"),
                    callbackUrl: null));
        }
    }
}