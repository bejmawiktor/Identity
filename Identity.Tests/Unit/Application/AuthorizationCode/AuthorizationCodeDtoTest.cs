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
        [Test]
        public void TestConstructing_WhenCodeGiven_ThenCodeIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true);

            Assert.That(authorizationCodeDto.Code, Is.EqualTo(authorizationCodeId.Code));
        }

        [Test]
        public void TestConstructing_WhenApplicationIdGiven_ThenApplicationIdIsSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true);

            Assert.That(authorizationCodeDto.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
        }

        [Test]
        public void TestConstructing_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);

            Assert.That(authorizationCodeDto.ExpiresAt, Is.EqualTo(expiresAt));
        }

        [Test]
        public void TestConstructing_WhenUsedGiven_ThenUsedIsSet()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);

            Assert.That(authorizationCodeDto.Used, Is.EqualTo(true));
        }

        [Test]
        public void TestEquals_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenTrueIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: false);

            Assert.That(firstAuthorizationCodeDto.Equals(secondAuthorizationCodeDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalAuthorizationCodesDtosGiven_ThenHashCodesAreEqual()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentAuthorizationCodesDtosGiven_ThenHashCodesAreNotEqual()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var firstAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);
            var secondAuthorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: applicationId.ToGuid(),
                expiresAt: expiresAt,
                used: false);

            Assert.That(firstAuthorizationCodeDto.GetHashCode(), Is.Not.EqualTo(secondAuthorizationCodeDto.GetHashCode()));
        }

        [Test]
        public void TestToAuthorizationCode_WhenConvertingToAuthorizationCode_ThenAuthorizationCodeIsReturned()
        {
            DateTime expiresAt = DateTime.Now;
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code,
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: expiresAt,
                used: true);

            AuthorizationCode authorizationCode = authorizationCodeDto.ToAuthorizationCode();

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Id.Code, Is.EqualTo(authorizationCodeId.Code));
                Assert.That(authorizationCode.Id.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(expiresAt));
                Assert.That(authorizationCode.Used, Is.True);
            });
        }
    }
}
