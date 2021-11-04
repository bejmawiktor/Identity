using Identity.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class SHA256CodeHashingAlgorithmTest
    {
        public static IEnumerable<object[]> WrongSizeTestData
        {
            get
            {
                yield return new object[] { new byte[4] };
                yield return new object[] { new byte[17] };
                yield return new object[] { new byte[47] };
                yield return new object[] { new byte[63] };
                yield return new object[] { new byte[255] };
            }
        }

        [Test]
        public void TestHash_WhenNullCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("code"),
                () => sha256CodeHashingAlgorithm.Hash(null));
        }

        [Test]
        public void TestHash_WhenCodeGiven_ThenHashedCodeIsReturned()
        {
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            byte[] hashedCode = sha256CodeHashingAlgorithm.Hash(Code.Generate());

            Assert.That(hashedCode, Is.Not.Empty);
        }

        [Test]
        public void TestHash_WhenMultipleTimesSameCodeIsEncrypted_ThenReturnedHashedCodesAreSame()
        {
            Code code = Code.Generate();
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            byte[] firstHashedCode = sha256CodeHashingAlgorithm.Hash(code);
            byte[] secondHashedCode = sha256CodeHashingAlgorithm.Hash(code);

            Assert.That(firstHashedCode, Is.EqualTo(secondHashedCode));
        }

        [Test]
        public void TestHash_WhenDifferentCodesIsEncrypted_ThenReturnedHashedCodesAreDifferent()
        {
            Code firstCode = Code.Generate();
            Code secondCode = Code.Generate();
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            byte[] firstHashedCode = sha256CodeHashingAlgorithm.Hash(firstCode);
            byte[] secondHashedCode = sha256CodeHashingAlgorithm.Hash(secondCode);

            Assert.That(firstHashedCode, Is.Not.EqualTo(secondHashedCode));
        }

        [Test]
        public void TestValidate_WhenNullHashedCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedCode"),
                () => sha256CodeHashingAlgorithm.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyHashedCodeGiven_ThenArgumentExceptionIsThrown()
        {
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Hashed code can't be empty."),
                () => sha256CodeHashingAlgorithm.Validate(Array.Empty<byte>()));
        }

        [TestCaseSource(nameof(WrongSizeTestData))]
        public void TestValidate_WhenWrongSizeHashedCodeGiven_ThenArgumentExceptionIsThrown(byte[] hashedCode)
        {
            var sha256CodeHashingAlgorithm = new SHA256CodeHashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong hashed code given."),
                () => sha256CodeHashingAlgorithm.Validate(hashedCode));
        }
    }
}