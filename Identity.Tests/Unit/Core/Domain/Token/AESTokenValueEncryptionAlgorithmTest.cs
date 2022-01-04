using Identity.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class AESTokenValueEncryptionAlgorithmTest
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
        public void TestEncrypt_WhenNullTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenValue"),
                () => aesTokenValueEncriptionAlgorithm.Encrypt(null));
        }

        [Test]
        public void TestEncrypt_WhenTokenValueGiven_ThenEncryptedTokenValueIsReturned()
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            byte[] encryptedTokenValue = aesTokenValueEncriptionAlgorithm.Encrypt(this.GetTokenValue());

            Assert.That(encryptedTokenValue, Is.Not.Empty);
        }

        private TokenValue GetTokenValue()
        {
            TokenInformation tokenInformation = new(
                Guid.NewGuid(),
                ApplicationId.Generate(),
                TokenType.Access,
                new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add"),
                    new PermissionId(new ResourceId("MyResource"), "Remove")
                },
                DateTime.Now);

            return TokenValueEncoder.Encode(tokenInformation);
        }

        [Test]
        public void TestEncrypt_WhenMultipleTimesSameTokenValueIsEncrypted_ThenReturnedEncryptedTokenValuesAreSame()
        {
            TokenValue tokenValue = this.GetTokenValue();
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            byte[] firstEncryptedTokenValue = aesTokenValueEncriptionAlgorithm.Encrypt(tokenValue);
            byte[] secondEncryptedTokenValue = aesTokenValueEncriptionAlgorithm.Encrypt(tokenValue);

            Assert.That(firstEncryptedTokenValue, Is.EqualTo(secondEncryptedTokenValue));
        }

        [Test]
        public void TestDecrypt_WhenNullEncryptedTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedTokenValue"),
                () => aesTokenValueEncriptionAlgorithm.Decrypt(null));
        }

        [Test]
        public void TestDecrypt_WhenEmptyEncryptedTokenValueGiven_ThenArgumentExceptionIsThrown()
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Encrypted token value can't be empty."),
                () => aesTokenValueEncriptionAlgorithm.Decrypt(Array.Empty<byte>()));
        }

        [Test]
        public void TestDecrypt_WhenEncryptedTokenValueGiven_ThenTokenValueIsReturned()
        {
            TokenValue tokenValue = this.GetTokenValue();
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();
            byte[] encryptedTokenValue = aesTokenValueEncriptionAlgorithm.Encrypt(tokenValue);

            TokenValue decryptedTokenValue = aesTokenValueEncriptionAlgorithm.Decrypt(encryptedTokenValue);

            Assert.That(decryptedTokenValue, Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestValidate_WhenNullEncryptedTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedTokenValue"),
                () => aesTokenValueEncriptionAlgorithm.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyEncryptedTokenValueGiven_ThenArgumentExceptionIsThrown()
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Encrypted token value can't be empty."),
                () => aesTokenValueEncriptionAlgorithm.Validate(Array.Empty<byte>()));
        }

        [TestCaseSource(nameof(WrongBlockSizeTestData))]
        public void TestValidate_WhenWrongBlockSizeEncryptedTokenValueGiven_ThenArgumentExceptionIsThrown(byte[] encryptedTokenValue)
        {
            AESTokenValueEncryptionAlgorithm aesTokenValueEncriptionAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Wrong encrypted token value given."),
                () => aesTokenValueEncriptionAlgorithm.Validate(encryptedTokenValue));
        }
    }
}