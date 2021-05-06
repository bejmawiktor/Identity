﻿using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using Application = Identity.Domain.Application;
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationTest
    {
        private static EncryptedSecretKey TestSecretKey = EncryptedSecretKey.Encrypt(SecretKey.Generate());

        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: TestSecretKey,
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
                    secretKey: TestSecretKey,
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
                secretKey: TestSecretKey,
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
                    secretKey: TestSecretKey,
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
                    secretKey: TestSecretKey,
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
                secretKey: TestSecretKey,
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
                    secretKey: TestSecretKey,
                    homepageUrl: null,
                    callbackUrl: new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstruction_WhenSecretKeyGiven_ThenSecretKeyIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: TestSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.That(application.SecretKey, Is.EqualTo(TestSecretKey));
        }

        [Test]
        public void TestConstruction_WhenNullSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("secretKey"),
                () => new Application(
                    id: applicationId,
                    userId: userId,
                    name: "MyApp",
                    secretKey: null,
                    homepageUrl: new Url("https://www.example.com/"),
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
                secretKey: TestSecretKey,
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
                    secretKey: TestSecretKey,
                    homepageUrl: new Url("https://www.example.com"),
                    callbackUrl: null));
        }

        [Test]
        public void TestDecryptSecretKey_WhenDecrypting_ThenSecretKeyIsReturned()
        {
            var secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: encryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));


            var decryptedSecretKey = application.DecryptSecretKey();

            Assert.That(decryptedSecretKey, Is.EqualTo(secretKey));
        }

        [Test]
        public void TestRegenerateSecretKey_WhenRegenerating_ThenSecretKeyIsDifferentThanPrevious()
        {
            var secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: encryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            application.RegenerateSecretKey();

            Assert.That(application.SecretKey, Is.Not.EqualTo(encryptedSecretKey));
        }

        [Test]
        public void TestRegenerateSecretKey_WhenRegenerating_ThenNewSecretKeyIsNotNull()
        {
            var secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: encryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            application.RegenerateSecretKey();

            Assert.That(application.SecretKey, Is.Not.Null);
        }
    }
}