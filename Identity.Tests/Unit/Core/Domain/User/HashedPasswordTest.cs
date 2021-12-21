using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class HashedPasswordTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructor_WhenNullStringHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => new HashedPassword((string)null));
        }

        [Test]
        public void TestConstructor_WhenEmptyStringHashedPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Hashed password can't be empty."),
                () => new HashedPassword(string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullBytesHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => new HashedPassword((byte[])null));
        }

        [Test]
        public void TestConstructor_WhenBase64HashedStringGiven_ThenToStringReturnsBased64HashedString()
        {
            string base64HashedPassword = HashedPasswordTest.TestPassword.ToString();
            var password = new HashedPassword(base64HashedPassword);

            Assert.That(password.ToString(), Is.EqualTo(base64HashedPassword));
        }

        [Test]
        public void TestConstructor_WhenHashedPasswordBytesGiven_ThenToByteArrayReturnsSameByteArray()
        {
            byte[] hashedPassowrdBytes = HashedPasswordTest.TestPassword.ToByteArray();
            var password = new HashedPassword(hashedPassowrdBytes);

            Assert.That(password.ToByteArray(), Is.EqualTo(hashedPassowrdBytes));
        }

        [Test]
        public void TestToString_WhenConvertingToString_ThenNotEmptyStringIsReturned()
        {
            HashedPassword password = HashedPasswordTest.TestPassword;

            Assert.That(password.ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestToByteArray_WhenConvertingToByteArray_ThenNotEmptyByteArrayIsReturned()
        {
            HashedPassword password = HashedPasswordTest.TestPassword;

            Assert.That(password.ToByteArray(), Is.Not.Empty);
        }

        [Test]
        public void TestHash_WhenStringGiven_ThenHashedPasswordIsReturned()
        {
            HashedPassword password = HashedPassword.Hash(new Password("MyPassword"));

            Assert.That(password, Is.TypeOf<HashedPassword>());
        }

        [Test]
        public void TestHash_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => HashedPassword.Hash(null));
        }

        [TestCase("MySimplePass123")]
        [TestCase("12345678")]
        [TestCase("!2#Asdfg;'p*&")]
        public void TestVerify_WhenCorrectPasswordGiven_ThenSuccessIsReturned(string verifiedPassword)
        {
            var password = new Password(verifiedPassword);
            HashedPassword hashedPassword = HashedPassword.Hash(password);

            PasswordVerificationResult result = hashedPassword.Verify(password);

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Success));
        }

        [TestCase("MySimplePass123", "asdgggaa")]
        [TestCase("12345678", "123456789")]
        [TestCase("!2#Asdfg;'p*&", "!2#Asdfg'p*&")]
        public void TestVerify_WhenIncorrectPasswordGiven_ThenFailedIsReturned(string verifiedPassword, string incorrectPassword)
        {
            HashedPassword password = HashedPassword.Hash(new Password(verifiedPassword));

            PasswordVerificationResult result = password.Verify(new Password(incorrectPassword));

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Failed));
        }

        [Test]
        public void TestVerify_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("verifiedPassword"),
                () => HashedPasswordTest.TestPassword.Verify(null));
        }
    }
}