using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class TokenVerificationResultTest
    {
        [Test]
        public void TestConstructor_WhenDefaultConstructorIsUsed_ThenFailedIsReturned()
        {
            var tokenVerificationResult = new TokenVerificationResult();

            Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Failed));
        }

        [Test]
        public void TestConstructor_WhenDefaultConstructorIsUsed_ThenMessageIsEmpty()
        {
            var tokenVerificationResult = new TokenVerificationResult();

            Assert.That(tokenVerificationResult.Message, Is.Empty);
        }

        [Test]
        public void TestDefault_WhenGettingDefault_ThenFailedIsReturned()
        {
            TokenVerificationResult tokenVerificationResult = TokenVerificationResult.Default;

            Assert.That(tokenVerificationResult, Is.EqualTo(TokenVerificationResult.Failed));
        }

        [Test]
        public void TestToString_WhenSuccessIsUsed_ThenSuccessStringIsReturned()
        {
            TokenVerificationResult tokenVerificationResult = TokenVerificationResult.Success;

            Assert.That(tokenVerificationResult.ToString(), Is.EqualTo(nameof(TokenVerificationResult.Success)));
        }

        [Test]
        public void TestToString_WhenFailedIsUsed_ThenFailedStringIsReturned()
        {
            TokenVerificationResult tokenVerificationResult = TokenVerificationResult.Failed;

            Assert.That(tokenVerificationResult.ToString(), Is.EqualTo(nameof(TokenVerificationResult.Failed)));
        }

        [Test]
        public void TestWithMessage_WhenMessageGiven_ThenTokenVerificationResultWithSameValueAndNewMessageIsReturned()
        {
            TokenVerificationResult tokenVerificationResultWithMessage = TokenVerificationResult.Failed
                .WithMessage("Test message.");

            Assert.Multiple(() =>
            {
                Assert.That(tokenVerificationResultWithMessage, Is.EqualTo(TokenVerificationResult.Failed));
                Assert.That(tokenVerificationResultWithMessage.Message, Is.EqualTo("Test message."));
            });
        }
    }
}