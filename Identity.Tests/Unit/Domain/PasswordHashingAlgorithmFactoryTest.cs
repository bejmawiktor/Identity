using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PasswordHashingAlgorithmFactoryTest
    {
        [Test]
        public void TestCreate_WhenOneGiven_ThenPbkdf2HashingAlgorithmIsReturned()
        {
            var passwordHashingAlgorithm = PasswordHashingAlgorithmFactory.Create(1);

            Assert.That(passwordHashingAlgorithm, Is.TypeOf<Pbkdf2HashingAlgorithm>());
        }

        [Test]
        public void TestCreate_WhenUnrecognizedSymbolGiven_ThenUnknownHashingPasswordAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownHashingPasswordAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm symbol given."),
                () => PasswordHashingAlgorithmFactory.Create(2));
        }

        [Test]
        public void TestConvertToAlgorithmSymbol_WhenPbkdf2HashingAlgorithmGiven_ThenOneIsReturned()
        {
            byte algorithmSymbol = PasswordHashingAlgorithmFactory.ConvertToAlgorithmSymbol(
                typeof(Pbkdf2HashingAlgorithm));

            Assert.That(algorithmSymbol, Is.EqualTo(1));
        }

        [Test]
        public void TestCreate_WhenUnrecognizedAlgorithmGiven_ThenUnknownHashingPasswordAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownHashingPasswordAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm type given."),
                () => PasswordHashingAlgorithmFactory.ConvertToAlgorithmSymbol(typeof(object)));
        }
    }
}