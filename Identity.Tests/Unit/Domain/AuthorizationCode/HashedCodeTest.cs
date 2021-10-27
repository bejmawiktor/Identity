using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class HashedCodeTest
    {
        private static readonly HashedCode TestCode = new HashedCode(new SHA256CodeHashingAlgorithm().Hash(Code.Generate()));

        [Test]
        public void TestConstructing_WhenNullHashedCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("base64HashedCode"),
                () => new HashedCode((string)null));
        }

        [Test]
        public void TestConstructing_WhenEmptyHashedCodeGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Hashed code can't be empty."),
                () => new HashedCode(string.Empty));
        }

        [Test]
        public void TestConstructing_WhenNullBytesHashedCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedCode"),
                () => new HashedCode((byte[])null));
        }

        [Test]
        public void TestConstructing_WhenBase64HashedStringGiven_ThenToStringReturnsBased64HashedString()
        {
            string base64HashedCode = HashedCodeTest.TestCode.ToString();
            var hashedCode = new HashedCode(base64HashedCode);

            Assert.That(hashedCode.ToString(), Is.EqualTo(base64HashedCode));
        }

        [Test]
        public void TestConstructing_WhenHashedCodeBytesGiven_ThenToByteArrayReturnsSameByteArray()
        {
            byte[] hashedCodeBytes = HashedCodeTest.TestCode.ToByteArray();
            var hashedCode = new HashedCode(hashedCodeBytes);

            Assert.That(hashedCode.ToByteArray(), Is.EqualTo(hashedCodeBytes));
        }

        [Test]
        public void TestHash_WhenNullCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("code"),
                () => HashedCode.Hash(null));
        }

        [Test]
        public void TestHash_WhenCodeGiven_ThenHashedCodeIsReturned()
        {
            var code = Code.Generate();
            var hashedCode = HashedCode.Hash(code);

            Assert.That(hashedCode, Is.Not.Null);
        }

        [Test]
        public void TestHash_WhenMultipleTimesSameCodeIsHashed_ThenReturnedHashedCodesAreSame()
        {
            var code = Code.Generate();

            HashedCode firstHashedCode = HashedCode.Hash(code);
            HashedCode secondHashedCode = HashedCode.Hash(code);

            Assert.That(firstHashedCode, Is.EqualTo(secondHashedCode));
        }

        [Test]
        public void TestHash_WhenDifferentCodesAreHashed_ThenReturnedHashedCodesAreDifferent()
        {
            var firstCode = Code.Generate();
            var secondCode = Code.Generate();

            HashedCode firstHashedCode = HashedCode.Hash(firstCode);
            HashedCode secondHashedCode = HashedCode.Hash(secondCode);

            Assert.That(firstHashedCode, Is.Not.EqualTo(secondHashedCode));
        }
    }
}
