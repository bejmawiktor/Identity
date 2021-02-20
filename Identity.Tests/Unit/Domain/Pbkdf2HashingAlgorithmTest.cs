using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class Pbkdf2HashingAlgorithmTest
    {
        [Test]
        public void TestHash_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => pbkdf2PasswordHashingAlgorithm.Hash(null));
        }

        [Test]
        public void TestHash_WhenEmptyPasswordGiven_ThenArgumentExceptionIsThrown()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Can't hash empty password."),
                () => pbkdf2PasswordHashingAlgorithm.Hash(string.Empty));
        }

        [Test]
        public void TestHash_WhenPasswordGiven_ThenHashedPasswordIsReturned()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash("MySecretPassword");

            Assert.That(hashedPassword, Is.Not.Empty);
        }

        [Test]
        public void TestHash_WhenMultipleTimesSamePasswordIsHashed_ThenReturnedHashedPasswordIsDiffrent()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            byte[] firstHashedPassword = pbkdf2PasswordHashingAlgorithm.Hash("MySecretPassword");
            byte[] secondHashedPassword = pbkdf2PasswordHashingAlgorithm.Hash("MySecretPassword");

            Assert.That(firstHashedPassword, Is.Not.EqualTo(secondHashedPassword));
        }

        [Test]
        public void TestVerify_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => pbkdf2PasswordHashingAlgorithm.Verify(null, "MySecretPassword"));
        }

        [Test]
        public void TestVerify_WhenNullVerifiedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("verifiedPassword"),
                () => pbkdf2PasswordHashingAlgorithm.Verify(pbkdf2PasswordHashingAlgorithm.Hash("MySecretPassword"), null));
        }

        [TestCase("MySimplePass123")]
        [TestCase("12345678")]
        [TestCase("!2#Asdfg;'p*&")]
        public void TestVerify_WhenCorrectPasswordGiven_ThenSuccessIsReturned(string verifiedPassword)
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();
            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(verifiedPassword);

            PasswordVerificationResult result = pbkdf2PasswordHashingAlgorithm.Verify(hashedPassword, verifiedPassword);

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Success));
        }

        [TestCase("MySimplePass123", "asdgggaa")]
        [TestCase("12345678", "123456789")]
        [TestCase("!2#Asdfg;'p*&", "!2#Asdfg'p*&")]
        public void TestVerify_WhenIncorrectPasswordGiven_ThenFailedIsReturned(string verifiedPassword, string incorrectPassword)
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();
            byte[] hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash(verifiedPassword);

            PasswordVerificationResult result = pbkdf2PasswordHashingAlgorithm.Verify(hashedPassword, incorrectPassword);

            Assert.That(result, Is.EqualTo(PasswordVerificationResult.Failed));
        }

        [Test]
        public void TestValidate_WhenNullHashedPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("hashedPassword"),
                () => pbkdf2PasswordHashingAlgorithm.Validate(null));
        }

        [Test]
        public void TestValidate_WhenHashedPasswordWithLenngthOtherThan48Given_ThenArgumentExceptionIsThrown([Range(0, 100, 10)] int hashedPasswordLength)
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect hashed password given."),
                () => pbkdf2PasswordHashingAlgorithm.Validate(new byte[hashedPasswordLength]));
        }

        [Test]
        public void TestValidate_WhenCorrectHashedPasswordGiven_ThenNoExceptionIsThrown()
        {
            var pbkdf2PasswordHashingAlgorithm = new Pbkdf2HashingAlgorithm();
            var hashedPassword = pbkdf2PasswordHashingAlgorithm.Hash("MySecretPassword");

            Assert.DoesNotThrow(
                () => pbkdf2PasswordHashingAlgorithm.Validate(hashedPassword));
        }
    }
}