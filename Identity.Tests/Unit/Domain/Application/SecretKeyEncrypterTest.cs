using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    public class SecretKeyEncrypterTest
    {
        [Test]
        public void TestEncrypt_WhenNullSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("secretKey"),
                () => SecretKeyEncrypter.Encrypt(null));
        }

        [Test]
        public void TestEncrypt_WhenSecretKeyGiven_ThenEncryptedSecretKeyIsReturned()
        {
            var secretKey = SecretKey.Generate();
            var encryptedSecretKey = SecretKeyEncrypter.Encrypt(secretKey);

            Assert.That(encryptedSecretKey, Is.Not.Null);
        }

        [Test]
        public void TestEncrypt_WhenMultipleTimesSameSecretKeyIsEncrypted_ThenReturnedEncryptedSecretKeysAreDifferent()
        {
            var secretKey = SecretKey.Generate();

            EncryptedSecretKey firstEncryptedSecretKey = SecretKeyEncrypter.Encrypt(secretKey);
            EncryptedSecretKey secondEncryptedSecretKey = SecretKeyEncrypter.Encrypt(secretKey);

            Assert.That(firstEncryptedSecretKey, Is.Not.EqualTo(secondEncryptedSecretKey));
        }

        [Test]
        public void TestDecrypt_WhenNullEncryptedSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedSecretKey"),
                () => SecretKeyEncrypter.Decrypt(null));
        }

        [Test]
        public void TestDecrypt_WhenEncryptedSecretKeyGiven_ThenSecretKeySameAsSourceSecretKeyIsReturned()
        {
            var secretKey = SecretKey.Generate();
            EncryptedSecretKey encryptedSecretKey = SecretKeyEncrypter.Encrypt(secretKey);

            var decryptedSecretKey = SecretKeyEncrypter.Decrypt(encryptedSecretKey);

            Assert.That(decryptedSecretKey, Is.EqualTo(secretKey));
        }

        [Test]
        public void TestDecrypt_WhenEncryptedKeysWhereCreatedFromOneSecretKey_ThenSameSecretKeysAreReturned()
        {
            var secretKey = SecretKey.Generate();
            EncryptedSecretKey firstEncryptedSecretKey = SecretKeyEncrypter.Encrypt(secretKey);
            EncryptedSecretKey secondEncryptedSecretKey = SecretKeyEncrypter.Encrypt(secretKey);

            SecretKey firstSecretKey = SecretKeyEncrypter.Decrypt(firstEncryptedSecretKey);
            SecretKey secondSecretKey = SecretKeyEncrypter.Decrypt(secondEncryptedSecretKey);

            Assert.That(firstSecretKey, Is.EqualTo(secondSecretKey));
        }

        [Test]
        public void TestValidate_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedSecretKey"),
                () => SecretKeyEncrypter.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyHashedPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect encrypted secret key given."),
                () => SecretKeyEncrypter.Validate(Array.Empty<byte>()));
        }

        [Test]
        public void TestValidate_WhenCorrectHashedPasswordGiven_ThenNoExceptionIsThrown()
        {
            var encryptedSecretKey = SecretKeyEncrypter.Encrypt(SecretKey.Generate());

            Assert.DoesNotThrow(
                () => SecretKeyEncrypter.Validate(encryptedSecretKey.ToByteArray()));
        }
    }
}