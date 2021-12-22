using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

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
            TokenInformation tokenInformation = this.GetTokenInformation();
            TokenValue tokenValue = TokenValueEncoder.Encode(tokenInformation);

            Assert.That(tokenValue, Is.Not.Null);
        }

        private TokenInformation GetTokenInformation(
            Guid? id = null,
            ApplicationId applicationId = null,
            TokenType tokenType = null,
            PermissionId[] permissions = null)
        {
            var permissionsReplacement = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            return new TokenInformation(
                id ?? Guid.NewGuid(),
                applicationId ?? ApplicationId.Generate(),
                tokenType ?? TokenType.Refresh,
                permissions ?? permissionsReplacement);
        }

        [Test]
        public void TestEncode_WhenMultipleTimesSameTokenInformationIsEncoded_ThenReturnedTokenValuesAreSame()
        {
            TokenInformation tokenInformation = this.GetTokenInformation();

            TokenValue firstEncodedTokenValue = TokenValueEncoder.Encode(tokenInformation);
            TokenValue secondEncodedTokenValue = TokenValueEncoder.Encode(tokenInformation);

            Assert.That(firstEncodedTokenValue, Is.EqualTo(secondEncodedTokenValue));
        }

        [Test]
        public void TestEncode_WhenDifferentTokenInformationIsEncoded_ThenReturnedTokenValuesAreDifferent()
        {
            TokenInformation firstTokenInformation = this.GetTokenInformation();
            TokenInformation secondTokenInformation = this.GetTokenInformation();

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
            TokenInformation tokenInformation = this.GetTokenInformation();
            TokenValue encodedTokenValue = TokenValueEncoder.Encode(tokenInformation);

            TokenInformation decodedTokenInformation = TokenValueEncoder.Decode(encodedTokenValue);

            Assert.That(decodedTokenInformation, Is.EqualTo(tokenInformation));
        }

        [Test]
        public void TestDecode_WhenTokenValuesWhereCreatedFromOneTokenInformation_ThenSameTokenValuesAreReturned()
        {
            TokenInformation tokenInformation = this.GetTokenInformation();
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
            TokenValue tokenValue = TokenValueEncoder.Encode(this.GetTokenInformation());

            Assert.DoesNotThrow(
                () => TokenValueEncoder.Validate(tokenValue.ToString()));
        }
    }
}