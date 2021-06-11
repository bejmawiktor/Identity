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

        [Test]
        public void TestGenerateTokens_WhenGenerating_ThenTokenPairIsReturnedWithUserId()
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

            TokenPair tokens = application.GenerateTokens();

            Assert.Multiple(() =>
            {
                Assert.That(tokens.AccessToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokens.RefreshToken.ApplicationId, Is.EqualTo(applicationId));
            });
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshing_ThenTokenPairIsReturnedWithApplicationId()
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
            var refreshToken = Token.GenerateRefreshToken(applicationId);

            TokenPair tokens = application.RefreshTokens(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(tokens.AccessToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokens.RefreshToken.ApplicationId, Is.EqualTo(applicationId));
            });
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshing_ThenTokenPairIsReturnedWithRefreshTokenExpirationDateSameAsPreviousRefreshTokenGiven()
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
            var dateTime = DateTime.Now.AddDays(1);
            var refreshToken = Token.GenerateRefreshToken(applicationId, dateTime);

            TokenPair tokens = application.RefreshTokens(refreshToken);

            Assert.That(tokens.RefreshToken.ExpiresAt, Is.EqualTo(dateTime).Within(1).Seconds);
        }

        [Test]
        public void TestRefreshTokens_WhenWrongApplicationIdRefreshTokenGiven_ThenInvalidTokenExceptionIsThrown()
        {
            var secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            ApplicationId applicationId = ApplicationId.Generate();
            ApplicationId wrongApplicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: encryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));
            var refreshToken = Token.GenerateRefreshToken(wrongApplicationId);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Wrong refresh token given."),
                () => application.RefreshTokens(refreshToken));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenWithFailedVerificationGiven_ThenInvalidTokenExceptionIsThrown()
        {
            var dateTime = DateTime.Now.AddDays(-1);
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

            var refreshToken = Token.GenerateRefreshToken(applicationId, dateTime);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>(),
                () => application.RefreshTokens(refreshToken));
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenGenerating_ThenAuthorizationCodeIsReturnedWithApplicationId()
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

            AuthorizationCode authorizationCode = application.GenerateAuthorizationCode();

            Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(applicationId));
        }
    }
}