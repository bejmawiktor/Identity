using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class EncryptedTokenValueTest
    {
        private static readonly EncryptedTokenValue TestTokenValue = TokenValueEncrypter.Encrypt(EncryptedTokenValueTest.GetTokenValue());

        [Test]
        public void TestConstructor_WhenNullEncryptedTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("base64EncryptedTokenValue"),
                () => new EncryptedTokenValue((string)null));
        }

        [Test]
        public void TestConstructor_WhenEmptyEncryptedTokenValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Encrypted token value can't be empty."),
                () => new EncryptedTokenValue(string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullBytesEncryptedTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("encryptedTokenValue"),
                () => new EncryptedTokenValue((byte[])null));
        }

        [Test]
        public void TestConstructor_WhenBase64HashedStringGiven_ThenToStringReturnsBased64HashedString()
        {
            string base64EncryptedTokenValue = EncryptedTokenValueTest.TestTokenValue.ToString();
            EncryptedTokenValue encryptedTokenValue = new(base64EncryptedTokenValue);

            Assert.That(encryptedTokenValue.ToString(), Is.EqualTo(base64EncryptedTokenValue));
        }

        [Test]
        public void TestConstructor_WhenEncryptedTokenValueBytesGiven_ThenToByteArrayReturnsSameByteArray()
        {
            byte[] encryptedTokenValueBytes = EncryptedTokenValueTest.TestTokenValue.ToByteArray();
            EncryptedTokenValue encryptedTokenValue = new(encryptedTokenValueBytes);

            Assert.That(encryptedTokenValue.ToByteArray(), Is.EqualTo(encryptedTokenValueBytes));
        }

        [Test]
        public void TestEncrypt_WhenTokenValueGiven_ThenEncryptedTokenValueIsReturned()
        {
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(EncryptedTokenValueTest.GetTokenValue());

            Assert.That(encryptedTokenValue, Is.TypeOf<EncryptedTokenValue>());
        }

        private static TokenValue GetTokenValue()
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
        public void TestEncrypt_WhenNullEncryptGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenValue"),
                () => EncryptedTokenValue.Encrypt(null));
        }

        [Test]
        public void TestDecrypt_WhenDecrypting_ThenTokenValueIsReturned()
        {
            TokenValue tokenValue = EncryptedTokenValueTest.GetTokenValue();
            EncryptedTokenValue encryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);

            TokenValue decryptedTokenValue = encryptedTokenValue.Decrypt();

            Assert.That(decryptedTokenValue, Is.EqualTo(tokenValue));
        }

        [Test]
        public void TestEquals_WhenSameEncryptedTokenValueGiven_ThenTrueIsReturned()
        {
            TokenValue tokenValue = EncryptedTokenValueTest.GetTokenValue();
            EncryptedTokenValue firstEncryptedTokenValue = EncryptedTokenValue.Encrypt(tokenValue);
            EncryptedTokenValue secondEncryptedTokenValue = new(firstEncryptedTokenValue.ToByteArray());

            Assert.That(firstEncryptedTokenValue.Equals((EncryptedTokenValue)secondEncryptedTokenValue), Is.True);
        }

        [Test]
        public void TestEquals_WhenDiffrentEncryptedTokenValueGiven_ThenFalseIsReturned()
        {
            TokenValue firstTokenValue = EncryptedTokenValueTest.GetTokenValue();
            TokenValue secondTokenValue = EncryptedTokenValueTest.GetTokenValue();
            EncryptedTokenValue firstEncryptedTokenValue = EncryptedTokenValue.Encrypt(firstTokenValue);
            EncryptedTokenValue secondEncryptedTokenValue = EncryptedTokenValue.Encrypt(secondTokenValue);

            Assert.That(firstEncryptedTokenValue.Equals((EncryptedTokenValue)secondEncryptedTokenValue), Is.False);
        }
    }
}