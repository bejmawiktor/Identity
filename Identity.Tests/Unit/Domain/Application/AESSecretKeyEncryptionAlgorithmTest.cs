using Identity.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class AESSecretKeyEncryptionAlgorithmTest
    {
        public static IEnumerable<object[]> WrongBlockSizeTestData
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
        public void TestEncrypt_WhenNullSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("secretKey"),
                () => aesSecretKeyEncriptionAlgorithm.Encrypt(null));
        }

        [Test]
        public void TestEncrypt_WhenSecretKeyGiven_ThenEncryptedSecretKeyIsReturned()
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            byte[] encryptedSecretKey = aesSecretKeyEncriptionAlgorithm.Encrypt(SecretKey.Generate());

            Assert.That(encryptedSecretKey, Is.Not.Empty);
        }

        [Test]
        public void TestEncrypt_WhenMultipleTimesSameSecretKeyIsEncrypted_ThenReturnedEncryptedSecretKeysAreDifferent()
        {
            var secretKey = SecretKey.Generate();
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            byte[] firstEncryptedSecretKey = aesSecretKeyEncriptionAlgorithm.Encrypt(secretKey);
            byte[] secondEncryptedSecretKey = aesSecretKeyEncriptionAlgorithm.Encrypt(secretKey);

            Assert.That(firstEncryptedSecretKey, Is.Not.EqualTo(secondEncryptedSecretKey));
        }

        [Test]
        public void TestDecrypt_WhenNullEncryptedSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedSecretKey"),
                () => aesSecretKeyEncriptionAlgorithm.Decrypt(null));
        }

        [Test]
        public void TestDecrypt_WhenEmptyEncryptedSecretKeyGiven_ThenArgumentExceptionIsThrown()
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Encrypted secret key can't be empty."),
                () => aesSecretKeyEncriptionAlgorithm.Decrypt(Array.Empty<byte>()));
        }

        [Test]
        public void TestDecrypt_WhenEncryptedSecretKeyGiven_ThenSecretKeyIsReturned()
        {
            var secretKey = SecretKey.Generate();
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();
            byte[] encryptedSecretKey = aesSecretKeyEncriptionAlgorithm.Encrypt(secretKey);

            SecretKey decryptedSecretKey = aesSecretKeyEncriptionAlgorithm.Decrypt(encryptedSecretKey);

            Assert.That(decryptedSecretKey, Is.EqualTo(secretKey));
        }

        [Test]
        public void TestValidate_WhenNullEncryptedSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedSecretKey"),
                () => aesSecretKeyEncriptionAlgorithm.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyEncryptedSecretKeyGiven_ThenArgumentExceptionIsThrown()
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Encrypted secret key can't be empty."),
                () => aesSecretKeyEncriptionAlgorithm.Validate(Array.Empty<byte>()));
        }

        [TestCaseSource(nameof(WrongBlockSizeTestData))]
        public void TestValidate_WhenWrongBlockSizeEncryptedSecretKeyGiven_ThenArgumentExceptionIsThrown(byte[] encryptedSecretKey)
        {
            var aesSecretKeyEncriptionAlgorithm = new AESSecretKeyEncryptionAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong encrypted security key given."),
                () => aesSecretKeyEncriptionAlgorithm.Validate(encryptedSecretKey));
        }
    }
}