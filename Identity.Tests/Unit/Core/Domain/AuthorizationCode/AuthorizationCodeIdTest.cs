using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    public class AuthorizationCodeIdTest
    {
        private static readonly HashedCode TestCode = HashedCode.Hash(Code.Generate());

        [Test]
        public void TestConstructor_WhenNullCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("code"),
              () => new AuthorizationCodeId(null, ApplicationId.Generate()));
        }

        [Test]
        public void TestConstructor_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("applicationId"),
              () => new AuthorizationCodeId(TestCode, null));
        }

        [Test]
        public void TestConstructor_WhenCodeGiven_ThenCodeIsSet()
        {
            AuthorizationCodeId authorizationCodeId = new(TestCode, ApplicationId.Generate());

            Assert.That(authorizationCodeId.Code, Is.EqualTo(TestCode));
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotNullAuthorizationCodeIdCodeIsReturned()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate(), out _);

            Assert.That(authorizationCodeId.Code, Is.Not.Null);
        }

        [Test]
        public void TestGenerate_WhenMultipleGenerated_ThenKeysHaveDifferentValues()
        {
            AuthorizationCodeId firstAuthorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate(), out _);
            AuthorizationCodeId secondAuthorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate(), out _);

            Assert.That(firstAuthorizationCodeId.Code, Is.Not.EqualTo(secondAuthorizationCodeId.Code));
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenCodeIsSet()
        {
            AuthorizationCodeId.Generate(ApplicationId.Generate(), out Code code);

            Assert.That(code, Is.Not.Null);
        }
    }
}