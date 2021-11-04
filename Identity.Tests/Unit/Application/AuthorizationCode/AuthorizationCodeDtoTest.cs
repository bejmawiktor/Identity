using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class AuthorizationCodeDtoTest
    {
        private static readonly (string ResourceId, string Name)[] TestPermissions = new (string ResourceId, string Name)[]
        {
            ("MyResource1", "Add"),
            ("MyResource2", "Add")
        };

        [Test]
        public void TestConstructor_WhenCodeGiven_ThenCodeIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true,
                permissions: TestPermissions);

            Assert.That(authorizationCodeDto.Code, Is.EqualTo(authorizationCodeId.Code.ToString()));
        }

        [Test]
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true,
                permissions: TestPermissions);

            Assert.That(authorizationCodeDto.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);

            Assert.That(authorizationCodeDto.ExpiresAt, Is.EqualTo(expiresAt));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);

            Assert.That(authorizationCodeDto.Used, Is.EqualTo(true));
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);

            Assert.That(authorizationCodeDto.Permissions, Is.EqualTo(TestPermissions));
        }

        [Test]
        public void TestEquals_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: false,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource1", "Add")
                });

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenHashCodesAreEqual()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenHashCodesAreNotEqual()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: false,
                permissions: TestPermissions);

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.Not.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestToAuthorizationCode_WhenConvertingToAuthorizationCode_ThenAuthorizationCodeIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true,
                permissions: TestPermissions);

            AuthorizationCode authorizationCode = authorizationCodeDto.ToAuthorizationCode();

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id.Code, Is.EqualTo(authorizationCodeId.Code));
                Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(expiresAt));
                Assert.That(authorizationCode.Used, Is.True);
                Assert.That(authorizationCode.Permissions, Is.EquivalentTo(new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource1"), "Add"),
                    new PermissionId(new ResourceId("MyResource2"), "Add")
                }));
            });
        }
    }
}