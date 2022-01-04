using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    public class TokenValueEncrypterTest
    {
        [Test]
        public void TestEncrypt_WhenNullTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenValue"),
                () => TokenValueEncrypter.Encrypt(null));
        }

        [Test]
        public void TestEncrypt_WhenTokenValueGiven_ThenEncryptedTokenValueIsReturned()
        {
            TokenValue tokenValue = this.GetTokenValue();
            EncryptedTokenValue encryptedTokenValue = TokenValueEncrypter.Encrypt(tokenValue);

            Assert.That(encryptedTokenValue, Is.Not.Null);
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

            EncryptedTokenValue firstEncryptedTokenValue = TokenValueEncrypter.Encrypt(tokenValue);
            EncryptedTokenValue secondEncryptedTokenValue = TokenValueEncrypter.Encrypt(tokenValue);

            Assert.That(firstEncryptedTokenValue, Is.EqualTo(secondEncryptedTokenValue));
        }

        [Test]
        public void TestDecrypt_WhenNullEncryptedTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedTokenValue"),
                () => TokenValueEncrypter.Decrypt(null));
        }

        [Test]
        public void TestDecrypt_WhenEncryptedTokenValueGiven_ThenTokenValueSameAsSourceTokenValueIsReturned()
        {
            TokenValue tokenValue = this.GetTokenValue();
            EncryptedTokenValue encryptedTokenValue = TokenValueEncrypter.Encrypt(tokenValue);

            TokenValue decryptedTokenValue = TokenValueEncrypter.Decrypt(encryptedTokenValue);

            Assert.That(decryptedTokenValue, Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestDecrypt_WhenEncryptedKeysWhereCreatedFromOneTokenValue_ThenSameTokenValuesAreReturned()
        {
            TokenValue tokenValue = this.GetTokenValue();
            EncryptedTokenValue firstEncryptedTokenValue = TokenValueEncrypter.Encrypt(tokenValue);
            EncryptedTokenValue secondEncryptedTokenValue = TokenValueEncrypter.Encrypt(tokenValue);

            TokenValue firstTokenValue = TokenValueEncrypter.Decrypt(firstEncryptedTokenValue);
            TokenValue secondTokenValue = TokenValueEncrypter.Decrypt(secondEncryptedTokenValue);

            Assert.That(firstTokenValue, Is.EqualTo(secondTokenValue));
        }

        [Test]
        public void TestValidate_WhenNullEncryptedTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedTokenValue"),
                () => TokenValueEncrypter.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyEncryptedTokenValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect encrypted token value given."),
                () => TokenValueEncrypter.Validate(Array.Empty<byte>()));
        }

        [Test]
        public void TestValidate_WhenCorrectEncryptedTokenValueGiven_ThenNoExceptionIsThrown()
        {
            EncryptedTokenValue encryptedTokenValue = TokenValueEncrypter.Encrypt(this.GetTokenValue());

            Assert.DoesNotThrow(
                () => TokenValueEncrypter.Validate(encryptedTokenValue.ToByteArray()));
        }
    }
}