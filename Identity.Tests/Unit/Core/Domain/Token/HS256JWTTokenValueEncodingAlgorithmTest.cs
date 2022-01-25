using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class HS256JWTTokenValueEncodingAlgorithmTest
    {
        [Test]
        public void TestEncode_WhenNullTokenInformationGiven_ThenArgumentNullExceptionIsThrown()
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("tokenInformation"),
                () => algorithm.Encode(null));
        }

        [Test]
        public void TestEncode_WhenTokenInformationGiven_ThenNotNullTokenIsReturned()
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;

            string token = algorithm.Encode(tokenInformation);

            Assert.That(token, Is.Not.Null);
        }

        [Test]
        public void TestEncode_WhenTokenInformationGiven_ThenNotEmptyTokenIsReturned()
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;

            string token = algorithm.Encode(tokenInformation);

            Assert.That(token, Is.Not.Empty);
        }

        [Test]
        public void TestEncode_WhenGeneratingTokenWithDifferentTokenInformation_ThenTokensAreDifferent()
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();
            TokenInformation firstTokenInformation = TokenInformationBuilder.DefaultTokenInformation;
            TokenInformation secondTokenInformation = new TokenInformationBuilder()
                .WithId(Guid.NewGuid())
                .Build();

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
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [TestCase("asfasfg")]
        [TestCase("")]
        public void TestValidate_WhenTokenWithInvalidApplicationIdGiven_ThenInvalidTokenExceptionIsThrown(string applicationId)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, applicationId),
                new Claim("tokenType", TokenType.Access),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenTokenWithoutIdGiven_ThenInvalidTokenExceptionIsThrown()
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, ApplicationId.Generate().ToString()),
                new Claim("tokenType", TokenType.Access),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenTokenWithEmptyIdGiven_ThenInvalidTokenExceptionIsThrown()
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.Empty.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, ApplicationId.Generate().ToString()),
                new Claim("tokenType", TokenType.Access),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

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
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("tokenType", TokenType.Access),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenTokenWithoutPermissionsGiven_ThenInvalidTokenExceptionIsThrown()
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, ApplicationId.Generate().ToString()),
                new Claim("tokenType", TokenType.Access)
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [TestCase("MyResource.12321.123123")]
        [TestCase("MyResource12321")]
        [TestCase("MyResource.12321 asdasdasd")]
        [TestCase("")]
        [TestCase("MyResource12321 MyResource12321.add")]
        [TestCase("MyResource12321.add,MyResource12321.add")]
        public void TestValidate_WhenTokenWithIncorrectPermissionsGiven_ThenInvalidTokenExceptionIsThrown(string permissions)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, ApplicationId.Generate().ToString()),
                new Claim("tokenType", TokenType.Access),
                new Claim("permissions", permissions)
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

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
            ApplicationId applicationId = ApplicationId.Generate();
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, applicationId.ToString()),
                new Claim("tokenType", tokenType),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new HS256JWTTokenValueEncodingAlgorithm();

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
            ApplicationId applicationId = ApplicationId.Generate();
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, applicationId.ToString()),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

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
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(HS256JWTTokenValueEncodingAlgorithm.SecretKey));
            SigningCredentials credentials = new(securityKey, securityAlgorithm ?? SecurityAlgorithms.HmacSha256);
            Claim[] settedClaims = claims ?? Array.Empty<Claim>();
            JwtSecurityToken token = new(
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
            ApplicationId applicationId = new ApplicationId(Guid.NewGuid());
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationId.ToString()),
                new Claim("tokenType", TokenType.Access.ToString()),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

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
            ApplicationId applicationId = new ApplicationId(Guid.NewGuid());
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationId.ToString()),
                new Claim("tokenType", TokenType.Access.ToString()),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

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
            ApplicationId applicationId = new ApplicationId(Guid.NewGuid());
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationId.ToString()),
                new Claim("tokenType", TokenType.Access.ToString()),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            string token = this.EncodeTestJWTToken(expiresAt: null, claims: claims);

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Validate(token));
        }

        [Test]
        public void TestValidate_WhenValidTokenGiven_ThenNoExceptionIsThrown()
        {
            ApplicationId applicationId = new ApplicationId(Guid.NewGuid());
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, applicationId.ToString()),
                new Claim("tokenType", TokenType.Access),
                new Claim("permissions", "MyResource.Add MyResource.Remove MyResource2.Update")
            };
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            string token = this.EncodeTestJWTToken(expiresAt: DateTime.Now, claims: claims);

            Assert.DoesNotThrow(() => algorithm.Validate(token));
        }

        [Test]
        public void TestEncode_WhenValidTokenGiven_ThenNoExceptionIsThrown()
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();
            TokenInformation tokenInformation = TokenInformationBuilder.DefaultTokenInformation;

            string token = algorithm.Encode(tokenInformation);

            Assert.DoesNotThrow(() => algorithm.Validate(token));
        }

        [TestCase("asddsgfklhgjdfklhjfdkljgkjhklf")]
        [TestCase("asddsgfklhgjdfkl124235sdfg")]
        [TestCase("")]
        [TestCase(null)]
        public void TestDecode_WhenInvalidTokenGiven_ThenInvalidTokenExceptionIsThrown(string token)
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();

            Assert.Throws(
                Is.InstanceOf<InvalidTokenException>()
                    .And.Message
                    .EqualTo("Invalid token given."),
                () => algorithm.Decode(token));
        }

        [Test]
        public void TestDecode_WhenValidTokenGiven_ThenTokenInforamtionIsReturned()
        {
            HS256JWTTokenValueEncodingAlgorithm algorithm = new();
            ApplicationId applicationId = ApplicationId.Generate();
            Guid id = Guid.NewGuid();
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove"),
                new PermissionId(new ResourceId("MyResource2"), "Remove")
            };
            TokenInformation tokenInformation = new TokenInformationBuilder()
                .WithId(id)
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithPermissions(permissions)
                .Build();
            string token = algorithm.Encode(tokenInformation);

            TokenInformation result = algorithm.Decode(token);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(id));
                Assert.That(result.ApplicationId, Is.EqualTo(applicationId));
                Assert.That(result.TokenType, Is.EqualTo(TokenType.Refresh));
                Assert.That(result.Permissions, Is.EquivalentTo(permissions));
                Assert.That(result.ExpirationDate, Is.EqualTo(TokenType.Refresh.GenerateExpirationDate()).Within(1).Hours);
            });
        }
    }
}