using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class Pbkdf2HashingAlgorithmTest
    {
        [Test]
        public void TestHash_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => pbkdf2PasswordHashingAlgorithm.Hash(null));
        }

        [Test]
        public void TestHash_WhenPasswordGiven_ThenHashedPasswordIsReturned()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(new Password("MySecretPassword"));

            Assert.That(hashedPassword, Is.Not.Empty);
        }

        [Test]
        public void TestHash_WhenMultipleTimesSamePasswordIsHashed_ThenReturnedHashedPasswordIsDiffrent()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            byte[] firstHashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(new Password("MySecretPassword"));
            byte[] secondHashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(new Password("MySecretPassword"));

            Assert.That(firstHashedPassword, Is.Not.EqualTo(secondHashedPassword));
        }

        [Test]
        public void TestVerify_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => pbkdf2PasswordHashingAlgorithm.Verify(null, new Password("MySecretPassword")));
        }

        [Test]
        public void TestVerify_WhenNullVerifiedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("verifiedPassword"),
                () => pbkdf2PasswordHashingAlgorithm.Verify(pbkdf2PasswordHashingAlgorithm.Hash(new Password("MySecretPassword")), null));
        }

        [TestCase("MySimplePass123")]
        [TestCase("12345678")]
        [TestCase("!2#Asdfg;'p*&")]
        public void TestVerify_WhenCorrectPasswordGiven_ThenSuccessIsReturned(string verifiedPassword)
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();
            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(new Password(verifiedPassword));

            PasswordVerificationResult result = pbkdf2PasswordHashingAlgorithm.Verify(hashedPassword, new Password(verifiedPassword));

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Success));
        }

        [TestCase("MySimplePass123", "asdgggaa")]
        [TestCase("12345678", "123456789")]
        [TestCase("!2#Asdfg;'p*&", "!2#Asdfg'p*&")]
        public void TestVerify_WhenIncorrectPasswordGiven_ThenFailedIsReturned(string verifiedPassword, string incorrectPassword)
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();
            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(new Password(verifiedPassword));

            PasswordVerificationResult result = pbkdf2PasswordHashingAlgorithm.Verify(hashedPassword, new Password(incorrectPassword));

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Failed));
        }

        [Test]
        public void TestValidate_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => pbkdf2PasswordHashingAlgorithm.Validate(null));
        }

        [Test]
        public void TestValidate_WhenHashedPasswordWithLengthOtherThan48Given_ThenArgumentExceptionIsThrown([Range(0, 100, 10)] int hashedPasswordLength)
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect hashed password given."),
                () => pbkdf2PasswordHashingAlgorithm.Validate(new byte[hashedPasswordLength]));
        }

        [Test]
        public void TestValidate_WhenCorrectHashedPasswordGiven_ThenNoExceptionIsThrown()
        {
            Pbkdf2HashingAlgorithm pbkdf2PasswordHashingAlgorithm = new();
            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(new Password("MySecretPassword"));

            Assert.DoesNotThrow(
                () => pbkdf2PasswordHashingAlgorithm.Validate(hashedPassword));
        }
    }
}