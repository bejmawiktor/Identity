using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    public class TokenValue : ValueObject<string>
    {
        private TokenInformation TokenInformation { get; }

        public ApplicationId ApplicationId => this.TokenInformation.ApplicationId;
        public TokenType Type => this.TokenInformation.TokenType;
        public DateTime ExpiresAt => this.TokenInformation.ExpirationDate;
        public IReadOnlyCollection<PermissionId> Permissions => this.TokenInformation.Permissions;
        public bool Expired => this.ExpiresAt < DateTime.Now;

        public TokenValue(string value) : base(value)
        {
            this.TokenInformation = TokenValueEncoder.Decode(this);
        }

        protected override void ValidateValue(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if(value == string.Empty)
            {
                throw new ArgumentException("Given token value can't be empty.");
            }

            TokenValueEncoder.Validate(value);
        }

        internal static TokenValue GenerateAccessToken(
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

            return TokenValueEncoder.Encode(new TokenInformation(
                id: Guid.NewGuid(),
                applicationId: applicationId,
                tokenType: TokenType.Access,
                permissions: permissions));
        }

        internal static TokenValue GenerateRefreshToken(
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

            return TokenValueEncoder.Encode(new TokenInformation(
                id: Guid.NewGuid(),
                applicationId: applicationId,
                tokenType: TokenType.Refresh,
                permissions: permissions,
                expirationDate: expiresAt));
        }
    }
}