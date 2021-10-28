using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PaswordHasherTest
    {
        [Test]
        public void TestHash_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => PasswordHasher.Hash(null));
        }

        [Test]
        public void TestHash_WhenPasswordGiven_ThenHashedPasswordIsReturned()
        {
            HashedPassword hashedPassword = PasswordHasher.Hash(new Password("MySecretPassword"));

            Assert.That(hashedPassword, Is.Not.Null);
        }

        [Test]
        public void TestVerify_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => PasswordHasher.Verify(null, new Password("MySecretPassword")));
        }

        [Test]
        public void TestVerify_WhenNullVerifiedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("verifiedPassword"),
                () => PasswordHasher.Verify(PasswordHasher.Hash(new Password("MySecretPassword")), null));
        }

        [TestCase("MySimplePass123")]
        [TestCase("12345678")]
        [TestCase("!2#Asdfg;'p*&")]
        public void TestVerify_WhenCorrectPasswordGiven_ThenSuccessIsReturned(string verifiedPassword)
        {
            HashedPassword hashedPassword = PasswordHasher.Hash(new Password(verifiedPassword));

            PasswordVerificationResult result = PasswordHasher.Verify(hashedPassword, new Password(verifiedPassword));

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Success));
        }

        [TestCase("MySimplePass123", "asdgggaa")]
        [TestCase("12345678", "123456789")]
        [TestCase("!2#Asdfg;'p*&", "!2#Asdfg'p*&")]
        public void TestVerify_WhenIncorrectPasswordGiven_ThenFailedIsReturned(string verifiedPassword, string incorrectPassword)
        {
            var hashedPassword = PasswordHasher.Hash(new Password(verifiedPassword));

            PasswordVerificationResult result = PasswordHasher.Verify(hashedPassword, new Password(incorrectPassword));

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Failed));
        }

        [Test]
        public void TestValidate_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => PasswordHasher.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyHashedPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect hashed password given."),
                () => PasswordHasher.Validate(Array.Empty<byte>()));
        }

        [Test]
        public void TestValidate_WhenCorrectHashedPasswordGiven_ThenNoExceptionIsThrown()
        {
            var hashedPassword = PasswordHasher.Hash(new Password("MySecretPassword"));

            Assert.DoesNotThrow(
                () => PasswordHasher.Validate(hashedPassword.ToByteArray()));
        }
    }
}