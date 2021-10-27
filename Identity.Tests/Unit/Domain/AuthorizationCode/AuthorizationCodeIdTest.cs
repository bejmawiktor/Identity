using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    public class AuthorizationCodeIdTest
    {
        private static readonly HashedCode TestCode = HashedCode.Hash(Code.Generate());

        [Test]
        public void TestConstructing_WhenNullCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("code"),
              () => new AuthorizationCodeId(null, ApplicationId.Generate()));
        }

        [Test]
        public void TestConstructing_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("applicationId"),
              () => new AuthorizationCodeId(TestCode, null));
        }

        [Test]
        public void TestConstructing_WhenCodeGiven_ThenCodeIsSet()
        {
            var authorizationCodeId = new AuthorizationCodeId(TestCode, ApplicationId.Generate());

            Assert.That(authorizationCodeId.Code, Is.EqualTo(TestCode));
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotNullAuthorizationCodeIdCodeIsReturned()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate());

            Assert.That(authorizationCodeId.Code, Is.Not.Null);
        }

        [Test]
        public void TestGenerate_WhenMultipleGenerated_ThenKeysHaveDifferentValues()
        {
            AuthorizationCodeId firstAuthorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate());
            AuthorizationCodeId secondAuthorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate());

            Assert.That(firstAuthorizationCodeId.Code, Is.Not.EqualTo(secondAuthorizationCodeId.Code));
        }
    }
}