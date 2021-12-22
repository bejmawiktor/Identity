using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Application
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

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
            AuthorizationCodeDto authorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid());

            Assert.That(authorizationCodeDto.Code, Is.EqualTo(authorizationCodeId.Code.ToString()));
        }

        private AuthorizationCodeDto GetAuthorizationCodeDto(
            string code = null,
            Guid? applicationId = null,
            DateTime? expiresAt = null,
            bool? used = null,
            IEnumerable<(string ResourceId, string Name)> permissions = null)
        {
            ApplicationId applicationIdReplacement = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationIdReplacement, out _);

            return new AuthorizationCodeDto(
                code: code ?? authorizationCodeId.Code.ToString(),
                applicationId: applicationId ?? applicationIdReplacement.ToGuid(),
                expiresAt: expiresAt ?? DateTime.Now,
                used: used ?? true,
                permissions: permissions ?? TestPermissions);
        }

        [Test]
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto authorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid());

            Assert.That(authorizationCodeDto.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            DateTime expiresAt = DateTime.Now;
            AuthorizationCodeDto authorizationCodeDto = this.GetAuthorizationCodeDto(expiresAt: expiresAt);

            Assert.That(authorizationCodeDto.ExpiresAt, Is.EqualTo(expiresAt));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            AuthorizationCodeDto authorizationCodeDto = this.GetAuthorizationCodeDto(used: true);

            Assert.That(authorizationCodeDto.Used, Is.True);
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            AuthorizationCodeDto authorizationCodeDto = this.GetAuthorizationCodeDto(permissions: TestPermissions);

            Assert.That(authorizationCodeDto.Permissions, Is.EqualTo(TestPermissions));
        }

        [Test]
        public void TestEquals_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto firstAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                expiresAt: expiresAt);
            AuthorizationCodeDto secondAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                expiresAt: expiresAt);

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto firstAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                permissions: TestPermissions);
            AuthorizationCodeDto secondAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: applicationId.ToGuid(),
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
            AuthorizationCodeDto firstAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                expiresAt: expiresAt);
            AuthorizationCodeDto secondAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                expiresAt: expiresAt);

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenHashCodesAreNotEqual()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto firstAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                used: true);
            AuthorizationCodeDto secondAuthorizationCodeDto = this.GetAuthorizationCodeDto(
                authorizationCodeId.Code.ToString(),
                applicationId.ToGuid(),
                used: false);

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.Not.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestToAuthorizationCode_WhenConvertingToAuthorizationCode_ThenAuthorizationCodeIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto authorizationCodeDto = new AuthorizationCodeDto(
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