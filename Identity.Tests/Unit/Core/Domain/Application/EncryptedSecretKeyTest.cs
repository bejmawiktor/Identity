using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class EncryptedSecretKeyTest
    {
        private static readonly EncryptedSecretKey TestSecretKey = SecretKeyEncrypter.Encrypt(SecretKey.Generate());

        [Test]
        public void TestConstructor_WhenNullEncryptedSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("base64EncryptedSecretKey"),
                () => new EncryptedSecretKey((string)null));
        }

        [Test]
        public void TestConstructor_WhenEmptyEncryptedSecretKeyGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Encrypted secret key can't be empty."),
                () => new EncryptedSecretKey(string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullBytesEncryptedSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedSecretKey"),
                () => new EncryptedSecretKey((byte[])null));
        }

        [Test]
        public void TestConstructor_WhenBase64HashedStringGiven_ThenToStringReturnsBased64HashedString()
        {
            string base64EncryptedSecretKey = EncryptedSecretKeyTest.TestSecretKey.ToString();
            EncryptedSecretKey encryptedSecretKey = new EncryptedSecretKey(base64EncryptedSecretKey);

            Assert.That(encryptedSecretKey.ToString(), Is.EqualTo(base64EncryptedSecretKey));
        }

        [Test]
        public void TestConstructor_WhenEncryptedSecretKeyBytesGiven_ThenToByteArrayReturnsSameByteArray()
        {
            byte[] encryptedSecretKeyBytes = EncryptedSecretKeyTest.TestSecretKey.ToByteArray();
            EncryptedSecretKey encryptedSecretKey = new EncryptedSecretKey(encryptedSecretKeyBytes);

            Assert.That(encryptedSecretKey.ToByteArray(), Is.EqualTo(encryptedSecretKeyBytes));
        }

        [Test]
        public void TestEncrypt_WhenSecretKeyGiven_ThenEncryptedSecretKeyIsReturned()
        {
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(SecretKey.Generate());

            Assert.That(encryptedSecretKey, Is.TypeOf<EncryptedSecretKey>());
        }

        [Test]
        public void TestEncrypt_WhenNullEncryptGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("secretKey"),
                () => EncryptedSecretKey.Encrypt(null));
        }

        [Test]
        public void TestDecrypt_WhenDecrypting_ThenSecretKeyIsReturned()
        {
            SecretKey secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = EncryptedSecretKey.Encrypt(secretKey);

            SecretKey decryptedSecretKey = encryptedSecretKey.Decrypt();

            Assert.That(decryptedSecretKey, Is.EqualTo(secretKey));
        }
    }
}