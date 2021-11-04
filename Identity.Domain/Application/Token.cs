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

        public ApplicationId ApplicationId => this.TokenInformation.ApplicationId;
        public TokenType Type => this.TokenInformation.TokenType;
        public DateTime ExpiresAt => this.TokenInformation.ExpirationDate;
        public IReadOnlyCollection<PermissionId> Permissions => this.TokenInformation.Permissions;

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

        internal static Token GenerateAccessToken(
            ApplicationId applicationId,
            IEnumerable<PermissionId> permissions)
        {
            if(applicationId == null)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            if(permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            return new Token(TokenGenerationAlgorithm.Encode(new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Access,
                permissions: permissions)));
        }

        internal static Token GenerateRefreshToken(
            ApplicationId applicationId,
            IEnumerable<PermissionId> permissions,
            DateTime? expiresAt = null)
        {
            if(applicationId == null)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            if(permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            return new Token(TokenGenerationAlgorithm.Encode(new TokenInformation(
                applicationId: applicationId,
                tokenType: TokenType.Refresh,
                permissions: permissions,
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