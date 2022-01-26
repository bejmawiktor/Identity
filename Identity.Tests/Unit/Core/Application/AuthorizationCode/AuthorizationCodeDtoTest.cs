using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;
using System;

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
            AuthorizationCodeDto authorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .Build();

            Assert.That(authorizationCodeDto.Code, Is.EqualTo(authorizationCodeId.Code.ToString()));
        }

        [Test]
        public void TestConstructor_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto authorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .Build();

            Assert.That(authorizationCodeDto.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            DateTime expiresAt = DateTime.Now;
            AuthorizationCodeDto authorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithExpiresAt(expiresAt)
                .Build();

            Assert.That(authorizationCodeDto.ExpiresAt, Is.EqualTo(expiresAt));
        }

        [Test]
        public void TestConstructor_WhenUsedGiven_ThenUsedIsSet()
        {
            AuthorizationCodeDto authorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithUsed(true)
                .Build();

            Assert.That(authorizationCodeDto.Used, Is.True);
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            AuthorizationCodeDto authorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithPermissions(TestPermissions)
                .Build();

            Assert.That(authorizationCodeDto.Permissions, Is.EqualTo(TestPermissions));
        }

        [Test]
        public void TestEquals_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto firstAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
               .WithCode(authorizationCodeId.Code.ToString())
               .WithApplicationId(applicationId.ToGuid())
               .WithExpiresAt(expiresAt)
               .Build();
            AuthorizationCodeDto secondAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .WithExpiresAt(expiresAt)
                .Build();

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto firstAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .WithPermissions(TestPermissions)
                .Build();
            AuthorizationCodeDto secondAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .WithPermissions(new (string ResourceId, string Name)[]
                {
                    ("MyResource1", "Add")
                })
                .Build();

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenHashCodesAreEqual()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            DateTime expiresAt = DateTime.Now;
            AuthorizationCodeDto firstAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
               .WithCode(authorizationCodeId.Code.ToString())
               .WithApplicationId(applicationId.ToGuid())
               .WithExpiresAt(expiresAt)
               .Build();
            AuthorizationCodeDto secondAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .WithExpiresAt(expiresAt)
                .Build();

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenHashCodesAreNotEqual()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId, out _);
            AuthorizationCodeDto firstAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .WithUsed(true)
                .Build();
            AuthorizationCodeDto secondAuthorizationCodeDto = new AuthorizationCodeDtoBuilder()
                .WithCode(authorizationCodeId.Code.ToString())
                .WithApplicationId(applicationId.ToGuid())
                .WithUsed(false)
                .Build();

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.Not.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestToAuthorizationCode_WhenConvertingToAuthorizationCode_ThenAuthorizationCodeIsReturned()
        {
            AuthorizationCodeDto authorizationCodeDto = AuthorizationCodeDtoBuilder.DefaultAuthorizationCodeDto;

            AuthorizationCode authorizationCode = authorizationCodeDto.ToAuthorizationCode();

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id.Code, Is.EqualTo(new HashedCode(authorizationCodeDto.Code)));
                Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(new ApplicationId(authorizationCodeDto.ApplicationId)));
                Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(authorizationCodeDto.ExpiresAt));
                Assert.That(authorizationCode.Used, Is.False);
                Assert.That(authorizationCode.Permissions, Is.EquivalentTo(AuthorizationCodeDtoBuilder.DefaultPermissions));
            });
        }
    }
}