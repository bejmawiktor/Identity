using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class HashedPasswordTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash("MyPassword");

        [Test]
        public void TestConstructing_WhenNullStringHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => new HashedPassword((string)null));
        }

        [Test]
        public void TestConstructing_WhenEmptyStringHashedPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Hashed password can't be empty."),
                () => new HashedPassword(string.Empty));
        }

        [Test]
        public void TestConstructing_WhenNullBytesHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => new HashedPassword((byte[])null));
        }

        [Test]
        public void TestConstructing_WhenBase64HashedStringGiven_ThenToStringReturnsBased64HashedString()
        {
            string base64HashedPassword = HashedPasswordTest.TestPassword.ToString();
            var password = new HashedPassword(base64HashedPassword);

            Assert.That(password.ToString(), Is.EqualTo(base64HashedPassword));
        }

        [Test]
        public void TestConstructing_WhenHashedPasswordBytesGiven_ThenToByteArrayReturnsSameByteArray()
        {
            byte[] hashedPassowrdBytes = HashedPasswordTest.TestPassword.ToByteArray();
            var password = new HashedPassword(hashedPassowrdBytes);

            Assert.That(password.ToByteArray(), Is.EqualTo(hashedPassowrdBytes));
        }

        [Test]
        public void TestToString_WhenConvertingToString_ThenStringIsReturned()
        {
            var password = HashedPasswordTest.TestPassword;

            Assert.That(password.ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestToByteArray_WhenConvertingToByteArray_ThenByteArrayIsReturned()
        {
            var password = HashedPasswordTest.TestPassword;

            Assert.That(password.ToByteArray(), Is.Not.Empty);
        }

        [Test]
        public void TestHash_WhenStringGiven_ThenPasswordIsReturned()
        {
            var password = HashedPasswordTest.TestPassword;

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

        [Test]
        public void TestHash_WhenEmptyPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Password can't be empty."),
                () => HashedPassword.Hash(string.Empty));
        }

        [TestCase("a")]
        [TestCase("as")]
        [TestCase("asd")]
        [TestCase("asdf")]
        [TestCase("asdfg")]
        [TestCase("asdfgh")]
        public void TestHash_WhenTooShortPasswordGiven_ThenArgumentExceptionIsThrown(string password)
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Password must be longer than 6 characters."),
                () => HashedPassword.Hash(password));
        }

        [TestCase("MySimplePass123")]
        [TestCase("12345678")]
        [TestCase("!2#Asdfg;'p*&")]
        public void TestVerify_WhenCorrectPasswordGiven_ThenSuccessIsReturned(string verifiedPassword)
        {
            var password = HashedPassword.Hash(verifiedPassword);

            PasswordVerificationResult result = password.Verify(verifiedPassword);

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Success));
        }

        [TestCase("MySimplePass123", "asdgggaa")]
        [TestCase("12345678", "123456789")]
        [TestCase("!2#Asdfg;'p*&", "!2#Asdfg'p*&")]
        public void TestVerify_WhenIncorrectPasswordGiven_ThenFailedIsReturned(string verifiedPassword, string incorrectPassword)
        {
            var password = HashedPassword.Hash(verifiedPassword);

            PasswordVerificationResult result = password.Verify(incorrectPassword);

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

        [Test]
        public void TestVerify_WhenEmptyPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Verified password can't be empty."),
                () => HashedPasswordTest.TestPassword.Verify(string.Empty));
        }
    }
}