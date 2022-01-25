using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationTest
    {
        private static EncryptedSecretKey TestSecretKey = EncryptedSecretKey.Encrypt(SecretKey.Generate());

        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            Application application = new ApplicationBuilder()
                .WithUserId(userId)
                .Build();

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
                    applicationId,
                    null,
                    "MyApp",
                    TestSecretKey,
                    new Url("https://www.example.com"),
                    new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            Application application = new ApplicationBuilder()
                .WithName("MyApp")
                .Build();

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
                    applicationId,
                    userId,
                    null,
                    TestSecretKey,
                    new Url("https://www.example.com"),
                    new Url("https://www.example.com/1")));
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
                    applicationId,
                    userId,
                    string.Empty,
                    TestSecretKey,
                    new Url("https://www.example.com"),
                    new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstructor_WhenHomepageUrlGiven_ThenHomepageUrlIsSet()
        {
            Application application = new ApplicationBuilder()
                .WithHompageUrl(new Url("https://www.example1.com"))
                .Build();

            Assert.That(application.HomepageUrl, Is.EqualTo(new Url("https://www.example1.com")));
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
                    applicationId,
                    userId,
                    "MyApp",
                    TestSecretKey,
                    null,
                    new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstructor_WhenSecretKeyGiven_ThenSecretKeyIsSet()
        {
            Application application = new ApplicationBuilder()
                .WithSecretKey(TestSecretKey)
                .Build();

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
                    applicationId,
                    userId,
                    "MyApp",
                    null,
                    new Url("https://www.example.com/"),
                    new Url("https://www.example.com/1")));
        }

        [Test]
        public void TestConstructor_WhenCallbackUrlGiven_ThenCallbackUrlIsSet()
        {
            Application application = new ApplicationBuilder()
                .WithCallbackUrl(new Url("https://www.example1.com/1"))
                .Build();

            Assert.That(application.CallbackUrl, Is.EqualTo(new Url("https://www.example1.com/1")));
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
                    applicationId,
                    userId,
                    "MyApp",
                    TestSecretKey,
                    new Url("https://www.example.com"),
                    null));
        }

        [Test]
        public void TestDecryptSecretKey_WhenDecrypting_ThenSecretKeyIsReturned()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            Application application = new ApplicationBuilder()
                .WithSecretKey(encryptedSecretKey)
                .Build();

            SecretKey decryptedSecretKey = application.DecryptSecretKey();

            Assert.That(decryptedSecretKey, Is.EqualTo(secretKey));
        }

        [Test]
        public void TestRegenerateSecretKey_WhenRegenerating_ThenSecretKeyIsDifferentThanPrevious()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            Application application = new ApplicationBuilder()
                .WithSecretKey(encryptedSecretKey)
                .Build();

            application.RegenerateSecretKey();

            Assert.That(application.SecretKey, Is.Not.EqualTo(encryptedSecretKey));
        }

        [Test]
        public void TestRegenerateSecretKey_WhenRegenerating_ThenNewSecretKeyIsNotNull()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);
            Application application = new ApplicationBuilder()
                .WithSecretKey(encryptedSecretKey)
                .Build();

            application.RegenerateSecretKey();

            Assert.That(application.SecretKey, Is.Not.Null);
        }

        [Test]
        public void TestCreateAuthorizationCode_WhenCreating_ThenAuthorizationCodeIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;

            AuthorizationCode authorizationCode = application.CreateAuthorizationCode(permissions, out Code code);

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(application.Id));
                Assert.That(authorizationCode.Permissions, Is.EqualTo(permissions));
                Assert.That(authorizationCode.Used, Is.False);
                Assert.That(code, Is.Not.Null);
            });
        }

        [Test]
        public void TestCreateAccessToken_WhenCreating_ThenAccessTokenIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;

            AccessToken accessToken = application.CreateAccessToken(permissions);

            Assert.Multiple(() =>
            {
                Assert.That(accessToken.ApplicationId, Is.EqualTo(application.Id));
                Assert.That(accessToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestCreateRefreshToken_WhenCreating_ThenRefreshTokenIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;

            RefreshToken refreshToken = application.CreateRefreshToken(permissions);

            Assert.Multiple(() =>
            {
                Assert.That(refreshToken.ApplicationId, Is.EqualTo(application.Id));
                Assert.That(refreshToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestRefreshAccessToken_WhenWrongApplicationRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application firstApplication = ApplicationBuilder.DefaultApplication;
            Application secondApplication = new ApplicationBuilder().WithId(ApplicationId.Generate()).Build();
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
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;
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
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(applicationId, permissions, DateTime.Now.AddDays(-1));
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .Build();
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
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;
            RefreshToken refreshToken = application.CreateRefreshToken(permissions);

            AccessToken accessToken = application.RefreshAccessToken(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(accessToken.ApplicationId, Is.EqualTo(application.Id));
                Assert.That(accessToken.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestRefreshRefreshToken_WhenWrongApplicationRefreshTokenGiven_ThenInvalidOperationExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application firstApplication = ApplicationBuilder.DefaultApplication;
            Application secondApplication = new ApplicationBuilder()
                .WithId(ApplicationId.Generate())
                .Build();
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
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;
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
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            TokenId tokenId = TokenId.GenerateRefreshTokenId(applicationId, permissions, DateTime.Now.AddDays(-1));
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .Build();
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
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            Application application = ApplicationBuilder.DefaultApplication;
            RefreshToken refreshToken = application.CreateRefreshToken(permissions);

            RefreshToken refreshedRefreshToken = application.RefreshRefreshToken(refreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(refreshedRefreshToken.ApplicationId, Is.EqualTo(application.Id));
                Assert.That(refreshedRefreshToken.Permissions, Is.EquivalentTo(permissions));
                Assert.That(refreshedRefreshToken.ExpiresAt, Is.EqualTo(refreshToken.ExpiresAt));
            });
        }
    }
}