using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class SecretKeyEncryptionAlgorithmFactoryTest
    {
        [Test]
        public void TestCreate_WhenOneGiven_ThenAESSecretKeyEncryptionAlgorithmIsReturned()
        {
            ISecretKeyEncryptionAlgorithm secretKeyEncryptionAlgorithm = SecretKeyEncryptionAlgorithmFactory.Create(1);

            Assert.That(secretKeyEncryptionAlgorithm, Is.TypeOf<AESSecretKeyEncryptionAlgorithm>());
        }

        [Test]
        public void TestCreate_WhenUnrecognizedSymbolGiven_ThenUnknownSecretKeyEncryptionAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownSecretKeyEncryptionAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm symbol given."),
                () => SecretKeyEncryptionAlgorithmFactory.Create(2));
        }

        [Test]
        public void TestConvertToAlgorithmSymbol_WhenAESSecretKeyEncryptionAlgorithmGiven_ThenOneIsReturned()
        {
            byte algorithmSymbol = SecretKeyEncryptionAlgorithmFactory.ConvertToAlgorithmSymbol(
                typeof(AESSecretKeyEncryptionAlgorithm));

            Assert.That(algorithmSymbol, Is.EqualTo(1));
        }

        [Test]
        public void TestCreate_WhenUnrecognizedAlgorithmGiven_ThenUnknownSecretKeyEncryptionAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownSecretKeyEncryptionAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm type given."),
                () => SecretKeyEncryptionAlgorithmFactory.ConvertToAlgorithmSymbol(typeof(object)));
        }
    }
}