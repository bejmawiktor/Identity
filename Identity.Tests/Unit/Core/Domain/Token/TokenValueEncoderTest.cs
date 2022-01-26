using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class TokenValueEncoderTest
    {
        [Test]
        public void TestEncode_WhenNullTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenInformation"),
                () => TokenValueEncoder.Encode(null));
        }

        [Test]
        public void TestEncode_WhenTokenValueGiven_ThenTokenValueIsReturned()
        {
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;
            TokenValue tokenValue = TokenValueEncoder.Encode(tokenInformation);

            Assert.That(tokenValue, Is.Not.Null);
        }

        [Test]
        public void TestEncode_WhenMultipleTimesSameTokenInformationIsEncoded_ThenReturnedTokenValuesAreSame()
        {
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;

            TokenValue firstEncodedTokenValue = TokenValueEncoder.Encode(tokenInformation);
            TokenValue secondEncodedTokenValue = TokenValueEncoder.Encode(tokenInformation);

            Assert.That(firstEncodedTokenValue, Is.EqualTo(secondEncodedTokenValue));
        }

        [Test]
        public void TestEncode_WhenDifferentTokenInformationIsEncoded_ThenReturnedTokenValuesAreDifferent()
        {
            TokenInformation firstTokenInformation = TokenInformationBuilder.DefaultTokenInformation;
            TokenInformation secondTokenInformation = new TokenInformationBuilder()
                .WithId(Guid.NewGuid())
                .Build();

            TokenValue firstEncodedTokenValue = TokenValueEncoder.Encode(firstTokenInformation);
            TokenValue secondEncodedTokenValue = TokenValueEncoder.Encode(secondTokenInformation);

            Assert.That(firstEncodedTokenValue, Is.Not.EqualTo(secondEncodedTokenValue));
        }

        [Test]
        public void TestDecode_WhenNullTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenValue"),
                () => TokenValueEncoder.Decode(null));
        }

        [Test]
        public void TestDecode_WhenTokenValueGiven_ThenTokenInformationSameAsSourceTokenInformationIsReturned()
        {
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;
            TokenValue encodedTokenValue = TokenValueEncoder.Encode(tokenInformation);

            TokenInformation decodedTokenInformation = TokenValueEncoder.Decode(encodedTokenValue);

            Assert.That(decodedTokenInformation, Is.EqualTo(tokenInformation));
        }

        [Test]
        public void TestDecode_WhenTokenValuesWhereCreatedFromOneTokenInformation_ThenSameTokenValuesAreReturned()
        {
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;
            TokenValue firstEncodedTokenValue = TokenValueEncoder.Encode(tokenInformation);
            TokenValue secondEncodedTokenValue = TokenValueEncoder.Encode(tokenInformation);

            TokenInformation firstTokenInformation = TokenValueEncoder.Decode(firstEncodedTokenValue);
            TokenInformation secondTokenInformation = TokenValueEncoder.Decode(secondEncodedTokenValue);

            Assert.That(firstTokenInformation, Is.EqualTo(secondTokenInformation));
        }

        [Test]
        public void TestValidate_WhenNullTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenValue"),
                () => TokenValueEncoder.Validate(null));
        }

        [Test]
        public void TestValidate_WhenEmptyTokenValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect token value given."),
                () => TokenValueEncoder.Validate(string.Empty));
        }

        [Test]
        public void TestValidate_WhenCorrectTokenValueGiven_ThenNoExceptionIsThrown()
        {
            TokenValue tokenValue = TokenValueEncoder.Encode(TokenInformationBuilder.DefaultTokenInformation);

            Assert.DoesNotThrow(
                () => TokenValueEncoder.Validate(tokenValue.ToString()));
        }
    }
}