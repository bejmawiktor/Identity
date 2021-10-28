using Identity.Domain;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class HS256JWTTokenGenerationAlgorithmTest
    {
        [Test]
        public void TestEncode_WhenNullTokenInformationGiven_ThenArgumentNullExceptionIsThrown()
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenInformation"),
                () => algorithm.Encode(null));
        }

        [Test]
        public void TestEncode_WhenTokenInformationGiven_ThenNotNullTokenIsReturned()
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();
            ApplicationId applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh);

            string token = algorithm.Encode(tokenInformation);

            Assert.That(token, Is.Not.Null);
        }

        [Test]
        public void TestEncode_WhenTokenInformationGiven_ThenNotEmptyTokenIsReturned()
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();
            ApplicationId applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh);

            string token = algorithm.Encode(tokenInformation);

            Assert.That(token, Is.Not.Empty);
        }

        [Test]
        public void TestEncode_WhenGeneratingTokenWithDifferentTokenInformation_ThenTokensAreDifferent()
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();
            DateTime expirationDate = DateTime.Now;
            ApplicationId firstApplicationId = ApplicationId.Generate();
            ApplicationId secondApplicationId = ApplicationId.Generate();
            var firstTokenInformation = new TokenInformation(
                firstApplicationId,
                TokenType.Refresh,
                expirationDate);
            var secondTokenInformation = new TokenInformation(
                secondApplicationId,
                TokenType.Refresh,
                expirationDate);

            string firstToken = algorithm.Encode(firstTokenInformation);
            string secondToken = algorithm.Encode(secondTokenInformation);

            Assert.That(firstToken, Is.Not.EqualTo(secondToken));
        }

        [TestCase("asddsgfklhgjdfklhjfdkljgkjhklf")]
        [TestCase("asddsgfklhgjdfkl124235sdfg")]
        [TestCase("")]
        [TestCase(null)]
        public void TestValidate_WhenInvalidFormatTokenGiven_ThenInvalidTokenExceptionIsThrown(string token)
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [TestCase("asfasfg")]
        [TestCase("")]
        public void TestValidate_WhenTokenWithInvalidUserIdGiven_ThenInvalidTokenExceptionIsThrown(string userId)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("tokenType", TokenType.Access)
            };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenTokenWithoutApplicationIdGiven_ThenInvalidTokenExceptionIsThrown()
        {
            var claims = new Claim[] { new Claim("tokenType", TokenType.Access) };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [TestCase("asfasfg")]
        [TestCase("")]
        public void TestValidate_WhenTokenWithInvalidTokenTypeGiven_ThenInvalidTokenExceptionIsThrown(string tokenType)
        {
            UserId userId = UserId.Generate();
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("tokenType", tokenType)
            };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenTokenWithoutTokenTypeGiven_ThenInvalidTokenExceptionIsThrown()
        {
            UserId userId = UserId.Generate();
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        private string EncodeTestJWTToken(
            DateTime? expiresAt = null,
            string issuer = "Identity",
            string audience = "Users",
            Claim[] claims = null,
            string securityAlgorithm = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(HS256JWTTokenGenerationAlgorithm.SecretKey));
            var credentials = new SigningCredentials(securityKey, securityAlgorithm ?? SecurityAlgorithms.HmacSha256);
            var settedClaims = claims ?? Array.Empty<Claim>();
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: settedClaims,
                notBefore: null,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [TestCase("asfasfg")]
        [TestCase("")]
        public void TestValidate_WhenWrongAudienceGiven_ThenInvalidTokenExceptionIsThrown(string audience)
        {
            UserId userId = new UserId(Guid.NewGuid());
            var claims = new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()) };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, audience: audience, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [TestCase("asfasfg")]
        [TestCase("")]
        public void TestValidate_WhenWrongIssuerGiven_ThenInvalidTokenExceptionIsThrown(string issuer)
        {
            UserId userId = new UserId(Guid.NewGuid());
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("tokenType", TokenType.Refresh.ToString())
            };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, issuer: issuer, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenTokenWithoutExpirationDateGiven_ThenInvalidTokenExceptionIsThrown()
        {
            UserId userId = new UserId(Guid.NewGuid());
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("tokenType", TokenType.Refresh.ToString())
            };
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            string token = this.EncodeTestJWTToken(expiresAt: null, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestEncode_WhenValidTokenGiven_ThenNoExceptionIsThrown()
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();
            UserId userId = UserId.Generate();
            ApplicationId applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh);

            string token = algorithm.Encode(tokenInformation);

            Assert.DoesNotThrow(() => algorithm.Validate(token));
        }

        [TestCase("asddsgfklhgjdfklhjfdkljgkjhklf")]
        [TestCase("asddsgfklhgjdfkl124235sdfg")]
        [TestCase("")]
        [TestCase(null)]
        public void TestDecode_WhenInvalidTokenGiven_ThenInvalidTokenExceptionIsThrown(string token)
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Decode(token));
        }

        [Test]
        public void TestDecode_WhenValidTokenGiven_ThenTokenInforamtionIsReturned()
        {
            var algorithm = new HS256JWTTokenGenerationAlgorithm();
            UserId userId = UserId.Generate();
            ApplicationId applicationId = ApplicationId.Generate();
            var tokenInformation = new TokenInformation(applicationId, TokenType.Refresh);
            string token = algorithm.Encode(tokenInformation);

            TokenInformation result = algorithm.Decode(token);

            Assert.Multiple(() =>
            {
                Assert.That(result.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(result.TokenType, Is.EqualTo(TokenType.Refresh));
                Assert.That(result.ExpirationDate, Is.EqualTo(TokenType.Refresh.GenerateExpirationDate()).Within(1).Hours);
            });
        }
    }
}