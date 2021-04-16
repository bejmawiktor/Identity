using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    public class Token : ValueObject
    {
        internal static readonly ITokenGenerationAlgorithm TokenGenerationAlgorithm
            = new HS256JWTTokenGenerationAlgorithm();

        private string Value { get; }
        private TokenInformation TokenInformation { get; }

        public UserId UserId => this.TokenInformation.UserId;
        public TokenType Type => this.TokenInformation.TokenType;
        public DateTime ExpiresAt => this.TokenInformation.ExpirationDate;

        public Token(string value)
        {
            this.ValidateValue(value);

            this.Value = value;
            this.TokenInformation = TokenGenerationAlgorithm.Decode(value);
        }

        private void ValidateValue(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if(value == string.Empty)
            {
                throw new ArgumentException("Given token value can't be empty.");
            }

            TokenGenerationAlgorithm.Validate(value);
        }

        internal static Token GenerateAccessToken(UserId userId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return new Token(TokenGenerationAlgorithm.Encode(new TokenInformation(
                userId: userId,
                tokenType: TokenType.Access)));
        }

        internal static Token GenerateRefreshToken(UserId userId, DateTime? expiresAt = null)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return new Token(TokenGenerationAlgorithm.Encode(new TokenInformation(
                userId: userId,
                tokenType: TokenType.Refresh,
                expirationDate: expiresAt)));
        }

        public TokenVerificationResult Verify()
        {
            if(DateTime.Now > this.ExpiresAt)
            {
                return TokenVerificationResult.Failed.WithMessage("Token has expired.");
            }

            return TokenVerificationResult.Success;
        }

        public override string ToString()
            => this.Value;

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Value;
        }
    }
}