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
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
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
        public void TestConstructor_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
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
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
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
        public void TestConstructor_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
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
        public void TestConstructor_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
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
        public void TestConstructor_WhenHomepageUrlGiven_ThenHomepageUrlIsSet()
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
        public void TestConstructor_WhenNullHomepageUrlGiven_ThenArgumentNullExceptionIsThrown()
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
        public void TestConstructor_WhenSecretKeyGiven_ThenSecretKeyIsSet()
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
        public void TestConstructor_WhenNullSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
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
        public void TestConstructor_WhenCallbackUrlGiven_ThenCallbackUrlIsSet()
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
        public void TestConstructor_WhenNullCallbackUrlGiven_ThenArgumentNullExceptionIsThrown()
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
            SecretKey secretKey = SecretKey.Generate();
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
            SecretKey secretKey = SecretKey.Generate();
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
            SecretKey secretKey = SecretKey.Generate();
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
        public void TestGenerateTokens_WhenGenerating_ThenTokenPairIsReturnedWithApplicationIdAndPermissions()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: encryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            TokenPair tokens = application.GenerateTokens(permissions);

            Assert.Multiple(() =>
            {
                Assert.That(tokens.AccessToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokens.RefreshToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokens.AccessToken.Permissions, Is.EquivalentTo(permissions));
                Assert.That(tokens.RefreshToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshing_ThenTokenPairIsReturnedWithApplicationId()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            SecretKey secretKey = SecretKey.Generate();
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
            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions);

            TokenPair tokens = application.RefreshTokens(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(tokens.AccessToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokens.RefreshToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(tokens.AccessToken.Permissions, Is.EquivalentTo(permissions));
                Assert.That(tokens.RefreshToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshing_ThenTokenPairIsReturnedWithRefreshTokenExpirationDateSameAsPreviousRefreshTokenGiven()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            SecretKey secretKey = SecretKey.Generate();
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
            DateTime dateTime = DateTime.Now.AddDays(1);
            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions, dateTime);

            TokenPair tokens = application.RefreshTokens(refreshToken);

            Assert.That(tokens.RefreshToken.ExpiresAt, Is.EqualTo(dateTime).Within(1).Seconds);
        }

        [Test]
        public void TestRefreshTokens_WhenWrongApplicationIdRefreshTokenGiven_ThenInvalidTokenExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            SecretKey secretKey = SecretKey.Generate();
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
            Token refreshToken = Token.GenerateRefreshToken(wrongApplicationId, permissions);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Wrong refresh token given."),
                () => application.RefreshTokens(refreshToken));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenWithFailedVerificationGiven_ThenInvalidTokenExceptionIsThrown()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            DateTime dateTime = DateTime.Now.AddDays(-1);
            SecretKey secretKey = SecretKey.Generate();
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

            Token refreshToken = Token.GenerateRefreshToken(applicationId, permissions, dateTime);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>(),
                () => application.RefreshTokens(refreshToken));
        }

        [Test]
        public void TestCreateAuthorizationCode_WhenCreating_ThenAuthorizationCodeIsReturned()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            ApplicationId applicationId = ApplicationId.Generate();
            UserId userId = UserId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            var application = new Application(
                id: applicationId,
                userId: userId,
                name: "MyApp",
                secretKey: encryptedSecretKey,
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            AuthorizationCode authorizationCode = application.CreateAuthorizationCode(permissions, out Code code);

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(authorizationCode.Permissions, Is.EqualTo(permissions));
                Assert.That(authorizationCode.Used, Is.False);
                Assert.That(code, Is.Not.Null);
            });
        }
    }
}