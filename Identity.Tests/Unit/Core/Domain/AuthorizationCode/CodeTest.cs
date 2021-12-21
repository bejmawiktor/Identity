using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class CodeTest
    {
        [Test]
        public void TestConstructor_WhenEmptyCodeGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Code can't be empty."),
              () => new Code(""));
        }

        [Test]
        public void TestConstructor_WhenNullCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("code"),
              () => new Code(null));
        }

        [TestCase("a")]
        [TestCase("aasdasdasdas")]
        [TestCase("afgdsgsdhtrhrthfhtrh")]
        [TestCase("afgdsgsdhtrhrthfhtrhasdasdsadaa")]
        [TestCase("afgdsgsdhtrhrthfhtrhasdasdsadfasfasfasfasfsagasgadgsdgsdhfgjhfjfdghjdf")]
        [TestCase("afgdsgsdhtrhrthfhtrhasdasdsadaa12")]
        public void TestConstructor_WhenIncorrectLengthCodeGiven_ThenArgumentExceptionIsThrown(string codeValue)
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Invalid code given."),
              () => new Code(codeValue));
        }

        [TestCase("tQh0Y0FlhhFW4rGpeowFVVQlsvat9xdR")]
        [TestCase("2RkHw9Ue1RitPYCnuKduabZKxMCOrUy5")]
        [TestCase("G1cDt0VeYFUvB5xnfslRoYEs2QLuE7wQ")]
        [TestCase("0b0gidi8UuqxWBwk0py9do7hiC3w15wb")]
        [TestCase("X659biBIpIH2z37eJMR6qV63hpVUOupA")]
        public void TestConstructor_WhenCorrectLengthCodeGiven_ThenCodeIsSet(string codeValue)
        {
            var code = new Code(codeValue);

            Assert.That(code.Value, Is.EqualTo(codeValue));
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotNullCodeIsReturned()
        {
            Code code = Code.Generate();

            Assert.That(code.Value, Is.Not.Null);
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotEmptyCodeIsReturned()
        {
            Code code = Code.Generate();

            Assert.That(code.Value, Is.Not.Empty);
        }

        [Test]
        public void TestGenerate_WhenMultipleGenerated_ThenCodesHaveDifferentValues()
        {
            Code firstCode = Code.Generate();
            Code secondCode = Code.Generate();

            Assert.That(firstCode, Is.Not.EqualTo(secondCode));
        }

        [Test]
        public void TestToString_WhenConverting_ThenValueIsReturned()
        {
            Code code = Code.Generate();

            Assert.That(code.ToString(), Is.EqualTo(code.Value));
        }
    }
}