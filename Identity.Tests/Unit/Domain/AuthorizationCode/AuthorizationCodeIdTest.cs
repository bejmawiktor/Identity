using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    public class AuthorizationCodeIdTest
    {
        [Test]
        public void TestConstructing_WhenEmptyCodeGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Code can't be empty."),
              () => new AuthorizationCodeId("", ApplicationId.Generate()));
        }

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
              () => new AuthorizationCodeId("pOPEy9Tq94aEss540azzC7xL6nCJDWto", null));
        }

        [TestCase("a")]
        [TestCase("aasdasdasdas")]
        [TestCase("afgdsgsdhtrhrthfhtrh")]
        [TestCase("afgdsgsdhtrhrthfhtrhasdasdsadaa")]
        [TestCase("afgdsgsdhtrhrthfhtrhasdasdsadfasfasfasfasfsagasgadgsdgsdhfgjhfjfdghjdf")]
        [TestCase("afgdsgsdhtrhrthfhtrhasdasdsadaa12")]
        public void TestConstructing_WhenIncorrectLengthCodeGiven_ThenArgumentExceptionIsThrown(string code)
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Invalid code given."),
              () => new AuthorizationCodeId(code, ApplicationId.Generate()));
        }

        [TestCase("tQh0Y0FlhhFW4rGpeowFVVQlsvat9xdR")]
        [TestCase("2RkHw9Ue1RitPYCnuKduabZKxMCOrUy5")]
        [TestCase("G1cDt0VeYFUvB5xnfslRoYEs2QLuE7wQ")]
        [TestCase("0b0gidi8UuqxWBwk0py9do7hiC3w15wb")]
        [TestCase("X659biBIpIH2z37eJMR6qV63hpVUOupA")]
        public void TestConstructing_WhenCorrectLengthCodeGiven_ThenCodeIsSet(string code)
        {
            var authorizationCodeId = new AuthorizationCodeId(code, ApplicationId.Generate());

            Assert.That(authorizationCodeId.Code, Is.EqualTo(code));
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotNullAuthorizationCodeIdCodeIsReturned()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate());

            Assert.That(authorizationCodeId.Code, Is.Not.Null);
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotEmptyAuthorizationCodeIdCodeIsReturned()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(ApplicationId.Generate());

            Assert.That(authorizationCodeId.Code, Is.Not.Empty);
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