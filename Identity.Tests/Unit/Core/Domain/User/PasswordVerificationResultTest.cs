using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PasswordVerificationResultTest
    {
        [Test]
        public void TestConstructor_WhenDefaultConstructorIsUsed_ThenFailedIsReturned()
        {
            var passwordVerificationResult = new PasswordVerificationResult();

            Assert.That(passwordVerificationResult, Is.EqualTo(PasswordVerificationResult.Failed));
        }

        [Test]
        public void TestDefault_WhenGettingDefault_ThenFailedIsReturned()
        {
            PasswordVerificationResult passwordVerificationResult = PasswordVerificationResult.Default;

            Assert.That(passwordVerificationResult, Is.EqualTo(PasswordVerificationResult.Failed));
        }

        [Test]
        public void TestToString_WhenSuccessIsUsed_ThenSuccessStringIsReturned()
        {
            PasswordVerificationResult passwordVerificationResult = PasswordVerificationResult.Success;

            Assert.That(passwordVerificationResult.ToString(), Is.EqualTo(nameof(PasswordVerificationResult.Success)));
        }

        [Test]
        public void TestToString_WhenFailedIsUsed_ThenFailedStringIsReturned()
        {
            PasswordVerificationResult passwordVerificationResult = PasswordVerificationResult.Failed;

            Assert.That(passwordVerificationResult.ToString(), Is.EqualTo(nameof(PasswordVerificationResult.Failed)));
        }
    }
}