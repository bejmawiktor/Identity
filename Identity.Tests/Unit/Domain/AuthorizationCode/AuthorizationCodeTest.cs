using Identity.Domain;
using NUnit.Framework;
using System;
using System.Linq;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    public class AuthorizationCodeTest
    {
        [Test]
        public void TestConstructor_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            DateTime now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                expiresAt: now,
                used: true,
                permissions: permissions);

            Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(now));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            DateTime now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                expiresAt: now,
                used: true,
                permissions: permissions);

            Assert.That(authorizationCode.Used, Is.True);
        }

        [Test]
        public void TestConstructor_WhenOnlyIdWithPermissionsGiven_ThenUsedIsSetToFalse()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                permissions: permissions);

            Assert.That(authorizationCode.Used, Is.False);
        }

        [Test]
        public void TestConstructor_WhenOnlyIdWithPermissionsGiven_ThenExpiresAtIsSetTo60SecondsAfterNow()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                permissions: permissions);

            Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(DateTime.Now.AddSeconds(60)).Within(5).Seconds);
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            DateTime now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                expiresAt: now,
                used: true,
                permissions: permissions);

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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            var authorizationCode = new AuthorizationCode(
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
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            var authorizationCode = AuthorizationCode.Create(ApplicationId.Generate(), permissions, out _);

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id, Is.Not.Null);
                Assert.That(authorizationCode.Permissions, Is.EquivalentTo(permissions));
            });
        }

        [Test]
        public void TestCreate_WhenCreating_ThenCodeIsReturned()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add")
            };
            AuthorizationCode authorizationCode = AuthorizationCode.Create(ApplicationId.Generate(), permissions, out Code code);

            Assert.That(code, Is.Not.Null);
        }

        [Test]
        public void TestUse_WhenAuthorizationCodeExpired_ThenInvalidOperationIsThrown()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                used: false,
                expiresAt: DateTime.Now.AddMinutes(-2),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Authorization code has expired."),
                () => authorizationCode.Use());
        }

        [Test]
        public void TestUse_WhenAuthorizationCodeWasPreviouslyUsed_ThenInvalidOperationIsThrown()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                used: true,
                expiresAt: DateTime.Now,
                permissions: new PermissionId[] 
                { 
                    new PermissionId(new ResourceId("MyResource"), "Add") 
                });

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Authorization code was used."),
                () => authorizationCode.Use());
        }

        [Test]
        public void TestUse_WhenUse_ThenUsedIsTrue()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });

            authorizationCode.Use();

            Assert.That(authorizationCode.Used, Is.True);
        }

        [Test]
        public void TestExpired_WhenExpiresAtElapsed_ThenTrueIsReturned()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                expiresAt: DateTime.Now.AddMinutes(-1),
                used: false,
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });

            Assert.That(authorizationCode.Expired, Is.True);
        }

        [Test]
        public void TestExpired_WhenExpiresAtNotElapsed_ThenFalseIsReturned()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate(), out _),
                expiresAt: DateTime.Now.AddMinutes(1),
                used: false,
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });

            Assert.That(authorizationCode.Expired, Is.False);
        }
    }
}