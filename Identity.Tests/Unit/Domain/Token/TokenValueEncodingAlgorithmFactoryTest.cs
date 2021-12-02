using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class TokenValueEncodingAlgorithmFactoryTest
    {
        [Test]
        public void TestCreate_WhenOneGiven_ThenHS256JWTTokenValueEncodingAlgorithmIsReturned()
        {
            ITokenValueEncodingAlgorithm tokenValueEncryptionAlgorithm = TokenValueEncodingAlgorithmFactory.Create(1);

            Assert.That(tokenValueEncryptionAlgorithm, Is.TypeOf<HS256JWTTokenValueEncodingAlgorithm>());
        }
        
        [Test]
        public void TestCreate_WhenUnrecognizedSymbolGiven_ThenUnknownTokenValueEncodingAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownTokenValueEncodingAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm symbol given."),
                () => TokenValueEncodingAlgorithmFactory.Create(2));
        }
        
        [Test]
        public void TestConvertToAlgorithmSymbol_WhenHS256JWTTokenValueEncodingAlgorithmGiven_ThenOneIsReturned()
        {
            byte algorithmSymbol = TokenValueEncodingAlgorithmFactory.ConvertToAlgorithmSymbol(
                typeof(HS256JWTTokenValueEncodingAlgorithm));

            Assert.That(algorithmSymbol, Is.EqualTo(1));
        }

        [Test]
        public void TestConvertToAlgorithmSymbol_WhenUnrecognizedAlgorithmGiven_ThenUnknownTokenValueEncryptionAlgorithmExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<UnknownTokenValueEncodingAlgorithmException>()
                    .And.Message
                    .EqualTo("Unrecognized algorithm type given."),
                () => TokenValueEncodingAlgorithmFactory.ConvertToAlgorithmSymbol(typeof(object)));
        }
    }
}
