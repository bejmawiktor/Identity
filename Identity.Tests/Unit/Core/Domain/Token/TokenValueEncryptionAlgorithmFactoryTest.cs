using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class TokenValueEncryptionAlgorithmFactoryTest
    {
        [Test]
        public void TestCreate_WhenOneGiven_ThenAESTokenValueEncryptionAlgorithmIsReturned()
        {
            ITokenValueEncryptionAlgorithm tokenValueEncryptionAlgorithm = TokenValueEncryptionAlgorithmFactory.Create(1);

            Assert.That(tokenValueEncryptionAlgorithm, Is.TypeOf<AESTokenValueEncryptionAlgorithm>());
        }

        [Test]
        public void TestCreate_WhenUnrecognizedSymbolGiven_ThenUnknownTokenValueEncryptionAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownTokenValueEncryptionAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm symbol given."),
                () => TokenValueEncryptionAlgorithmFactory.Create(2));
        }

        [Test]
        public void TestConvertToAlgorithmSymbol_WhenAESTokenValueEncryptionAlgorithmGiven_ThenOneIsReturned()
        {
            byte algorithmSymbol = TokenValueEncryptionAlgorithmFactory.ConvertToAlgorithmSymbol(
                typeof(AESTokenValueEncryptionAlgorithm));

            Assert.That(algorithmSymbol, Is.EqualTo(1));
        }

        [Test]
        public void TestConvertToAlgorithmSymbol_WhenUnrecognizedAlgorithmGiven_ThenUnknownTokenValueEncryptionAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownTokenValueEncryptionAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm type given."),
                () => TokenValueEncryptionAlgorithmFactory.ConvertToAlgorithmSymbol(typeof(object)));
        }
    }
}