using NUnit.Framework;
using System;
using Identity.Persistence.MSSQL.DataModels;
using Identity.Application;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using ApplicationId = Identity.Domain.ApplicationId;
    using AuthorizationCodeId = Identity.Domain.AuthorizationCodeId;

    [TestFixture]
    public class AuthorizationCodeTest
    {
        [Test]
        public void TestConstructing_WhenDtoGiven_ThenMembersAreSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            DateTime now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                new AuthorizationCodeDto(
                    authorizationCodeId.Code, 
                    authorizationCodeId.ApplicationId.ToGuid(),
                    now, 
                    true));

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Code, Is.EqualTo(authorizationCodeId.Code));
                Assert.That(authorizationCode.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
                Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(now));
                Assert.That(authorizationCode.Used, Is.True);
            });
        }

        [Test]
        public void TestSetFields_WhenDtoGiven_ThenMembersAreSet()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            DateTime now = DateTime.Now;
            var authorizationCode = new AuthorizationCode();

            authorizationCode.SetFields(new AuthorizationCodeDto(
                authorizationCodeId.Code,
                authorizationCodeId.ApplicationId.ToGuid(),
                now,
                true));

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCode.Code, Is.EqualTo(authorizationCodeId.Code));
                Assert.That(authorizationCode.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
                Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(now));
                Assert.That(authorizationCode.Used, Is.True);
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenAuthorizationCodeDtoIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(applicationId);
            DateTime now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                new AuthorizationCodeDto(
                    authorizationCodeId.Code,
                    authorizationCodeId.ApplicationId.ToGuid(),
                    now,
                    true));

            AuthorizationCodeDto authorizationCodeDto = authorizationCode.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(authorizationCodeDto.Code, Is.EqualTo(authorizationCodeId.Code));
                Assert.That(authorizationCodeDto.ApplicationId, Is.EqualTo(applicationId.ToGuid()));
                Assert.That(authorizationCodeDto.ExpiresAt, Is.EqualTo(now));
                Assert.That(authorizationCodeDto.Used, Is.True);
            });
        }
    }
}
