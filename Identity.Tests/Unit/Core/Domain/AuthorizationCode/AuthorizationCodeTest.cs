using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;
using System.Linq;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    public class AuthorizationCodeTest
    {
        [Test]
        public void TestConstructor_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            DateTime now = DateTime.Now;
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithExpiresAt(now)
                .Build();

            Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(now));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithUsed(true)
                .Build();

            Assert.That(authorizationCode.Used, Is.True);
        }

        [Test]
        public void TestConstructor_WhenOnlyIdWithPermissionsGiven_ThenUsedIsSetToFalse()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            AuthorizationCode authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                permissions: permissions);

            Assert.That(authorizationCode.Used, Is.False);
        }

        [Test]
        public void TestConstructor_WhenOnlyIdWithPermissionsGiven_ThenExpiresAtIsSetTo60SecondsAfterNow()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            AuthorizationCode authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                permissions: permissions);

            Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(DateTime.Now.AddSeconds(60)).Within(5).Seconds);
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithPermissions(permissions)
                .Build();

            Assert.That(authorizationCode.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissions"),
                () => new AuthorizationCode(
                    id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                    expiresAt: DateTime.Now,
                    used: true,
                    permissions: null));
        }

        [Test]
        public void TestConstructor_WhenEmptyPermissionsGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Can't create authorization code without permissions."),
                () => new AuthorizationCode(
                    id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                    expiresAt: DateTime.Now,
                    used: true,
                    permissions: Enumerable.Empty<PermissionId>()));
        }

        [Test]
        public void TestConstructor_WhenOnlyIdWithPermissionsGiven_ThenPermissionsAreSet()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            AuthorizationCode authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                permissions: permissions);

            Assert.That(authorizationCode.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenOnlyIdAndNullPermissionsGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissions"),
                () => new AuthorizationCode(
                    id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                    permissions: null));
        }

        [Test]
        public void TestConstructor_WhenOnlyIdAndEmptyPermissionsGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Can't create authorization code without permissions."),
                () => new AuthorizationCode(
                    id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                    permissions: Enumerable.Empty<PermissionId>()));
        }

        [Test]
        public void TestCreate_WhenCreating_ThenAuthorizationCodeIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            AuthorizationCode authorizationCode = AuthorizationCode.Create(ApplicationId.Generate(), permissions, out _);

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id, Is.Not.Null);
                Assert.That(authorizationCode.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestCreate_WhenCreating_ThenCodeIsReturned()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };

            AuthorizationCode.Create(ApplicationId.Generate(), permissions, out Code code);

            Assert.That(code, Is.Not.Null);
        }

        [Test]
        public void TestUse_WhenAuthorizationCodeExpired_ThenInvalidOperationIsThrown()
        {
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithUsed(false)
                .WithExpiresAt(DateTime.Now.AddMinutes(-2))
                .Build();

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Authorization code has expired."),
                () => authorizationCode.Use());
        }

        [Test]
        public void TestUse_WhenAuthorizationCodeWasPreviouslyUsed_ThenInvalidOperationIsThrown()
        {
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithUsed(true)
                .Build();

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Authorization code was used."),
                () => authorizationCode.Use());
        }

        [Test]
        public void TestUse_WhenUse_ThenUsedIsTrue()
        {
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();

            authorizationCode.Use();

            Assert.That(authorizationCode.Used, Is.True);
        }

        [Test]
        public void TestExpired_WhenExpiresAtElapsed_ThenTrueIsReturned()
        {
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithExpiresAt(DateTime.Now.AddDays(-1))
                .Build();

            Assert.That(authorizationCode.Expired, Is.True);
        }

        [Test]
        public void TestExpired_WhenExpiresAtNotElapsed_ThenFalseIsReturned()
        {
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();

            Assert.That(authorizationCode.Expired, Is.False);
        }
    }
}