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
            UserId userId = UserId.Generate();

            Application application = this.GetApplication(
                userId: userId);

            Assert.That(application.UserId, Is.EqualTo(userId));
        }

        private Application GetApplication(
            ApplicationId applicationId = null,
            UserId userId = null,
            string name = null,
            EncryptedSecretKey secretKey = null,
            Url homepageUrl = null,
            Url callbackUrl = null)
        {
            return new Application(
                id: applicationId ?? ApplicationId.Generate(),
                userId: userId ?? UserId.Generate(),
                name: name ?? "MyApp",
                secretKey: secretKey ?? TestSecretKey,
                homepageUrl: homepageUrl ?? new Url("https://www.example.com"),
                callbackUrl: callbackUrl ?? new Url("https://www.example.com/1"));
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
            Application application = this.GetApplication(name: "MyApp");

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
            Application application = GetApplication(homepageUrl: new Url("https://www.example.com"));

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
            Application application = this.GetApplication(secretKey: TestSecretKey);

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
            Application application = this.GetApplication(callbackUrl: new Url("https://www.example.com/1"));

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
            Application application = this.GetApplication(secretKey: encryptedSecretKey);

            SecretKey decryptedSecretKey = application.DecryptSecretKey();

            Assert.That(decryptedSecretKey, Is.EqualTo(secretKey));
        }

        [Test]
        public void TestRegenerateSecretKey_WhenRegenerating_ThenSecretKeyIsDifferentThanPrevious()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            Application application = this.GetApplication(
                secretKey: encryptedSecretKey);

            application.RegenerateSecretKey();

            Assert.That(application.SecretKey, Is.Not.EqualTo(encryptedSecretKey));
        }

        [Test]
        public void TestRegenerateSecretKey_WhenRegenerating_ThenNewSecretKeyIsNotNull()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            Application application = this.GetApplication(
                secretKey: encryptedSecretKey);

            application.RegenerateSecretKey();

            Assert.That(application.SecretKey, Is.Not.Null);
        }

        [Test]
        public void TestCreateAuthorizationCode_WhenCreating_ThenAuthorizationCodeIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);

            AuthorizationCode authorizationCode = application.CreateAuthorizationCode(permissions, out Code code);

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(authorizationCode.Permissions, Is.EqualTo(permissions));
                Assert.That(authorizationCode.Used, Is.False);
                Assert.That(code, Is.Not.Null);
            });
        }

        [Test]
        public void TestCreateAccessToken_WhenCreating_ThenAccessTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);

            AccessToken accessToken = application.CreateAccessToken(permissions);

            Assert.Multiple(() =>
            {
                Assert.That(accessToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(accessToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestCreateRefreshToken_WhenCreating_ThenRefreshTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);

            RefreshToken refreshToken = application.CreateRefreshToken(permissions);

            Assert.Multiple(() =>
            {
                Assert.That(refreshToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(refreshToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestRefreshAccessToken_WhenWrongApplicationRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            ApplicationId firstApplicationId = ApplicationId.Generate();
            ApplicationId secondApplicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application firstApplication = this.GetApplication(firstApplicationId);
            Application secondApplication = this.GetApplication(secondApplicationId);
            RefreshToken refreshToken = firstApplication.CreateRefreshToken(permissions);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Wrong refresh token given."),
                () => secondApplication.RefreshAccessToken(refreshToken));
        }

        [Test]
        public void TestRefreshAccessToken_WhenUsedRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);
            RefreshToken refreshToken = application.CreateRefreshToken(permissions);
            refreshToken.Use();

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Token was used before."),
                () => application.RefreshAccessToken(refreshToken));
        }

        [Test]
        public void TestRefreshAccessToken_WhenExpiredRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(applicationId, permissions, DateTime.Now.AddDays(-1));
            Application application = this.GetApplication(applicationId);
            RefreshToken refreshToken = new RefreshToken(tokenId);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Token has expired."),
                () => application.RefreshAccessToken(refreshToken));
        }

        [Test]
        public void TestRefreshAccessToken_WhenRefreshTokenGiven_ThenAccessTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);
            RefreshToken refreshToken = application.CreateRefreshToken(permissions);

            AccessToken accessToken = application.RefreshAccessToken(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(accessToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(accessToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestRefreshRefreshToken_WhenWrongApplicationRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            ApplicationId firstApplicationId = ApplicationId.Generate();
            ApplicationId secondApplicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application firstApplication = this.GetApplication(firstApplicationId);
            Application secondApplication = this.GetApplication(secondApplicationId);
            RefreshToken refreshToken = firstApplication.CreateRefreshToken(permissions);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Wrong refresh token given."),
                () => secondApplication.RefreshRefreshToken(refreshToken));
        }

        [Test]
        public void TestRefreshRefreshToken_WhenUsedRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);
            RefreshToken refreshToken = application.CreateRefreshToken(permissions);
            refreshToken.Use();

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Token was used before."),
                () => application.RefreshRefreshToken(refreshToken));
        }

        [Test]
        public void TestRefreshRefreshToken_WhenExpiredRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            var tokenId = TokenId.GenerateRefreshTokenId(applicationId, permissions, DateTime.Now.AddDays(-1));
            Application application = this.GetApplication(applicationId);
            RefreshToken refreshToken = new RefreshToken(tokenId);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Token has expired."),
                () => application.RefreshRefreshToken(refreshToken));
        }

        [Test]
        public void TestRefreshRefreshToken_WhenRefreshTokenGiven_ThenRefreshTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = this.GetApplication(applicationId);
            RefreshToken refreshToken = application.CreateRefreshToken(permissions);

            RefreshToken refreshedRefreshToken = application.RefreshRefreshToken(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(refreshedRefreshToken.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(refreshedRefreshToken.Permissions, Is.EquivalentTo(permissions));
                Assert.That(refreshedRefreshToken.ExpiresAt, Is.EqualTo(refreshToken.ExpiresAt));
            });
        }
    }
}