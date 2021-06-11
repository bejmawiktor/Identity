using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    public class AuthorizationCodeTest
    {
        [Test]
        public void TestConstruction_WhenExpiresAtGiven_ThenExpiresAtIsSet()
        {
            var now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate()),
                expiresAt: now,
                used: true);

            Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(now));
        }

        [Test]
        public void TestConstruction_WhenUsedGiven_ThenUsedIsSet()
        {
            var now = DateTime.Now;
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate()),
                expiresAt: now,
                used: true);

            Assert.That(authorizationCode.Used, Is.True);
        }

        [Test]
        public void TestConstruction_WhenOnlyIdGiven_ThenUsedIsSetToFalse()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate()));

            Assert.That(authorizationCode.Used, Is.False);
        }

        [Test]
        public void TestConstruction_WhenOnlyIdGiven_ThenExpiresAtIsSetTo60SecondsAfterNow()
        {
            var authorizationCode = new AuthorizationCode(
                id: AuthorizationCodeId.Generate(ApplicationId.Generate()));

            Assert.That(authorizationCode.ExpiresAt, Is.EqualTo(DateTime.Now.AddSeconds(60)).Within(5).Seconds);
        }
    }
}