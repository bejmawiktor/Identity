using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain
{
    public class TokenId : Identifier<EncryptedTokenValue, TokenId>
    {
        private IEnumerable<PermissionId> permissions;

        public ApplicationId ApplicationId { get; }
        public TokenType Type { get; }
        public DateTime ExpiresAt { get; }
        public IReadOnlyCollection<PermissionId> Permissions 
            => this.permissions.ToList().AsReadOnly();
        public bool Expired => this.ExpiresAt < DateTime.Now;

        public TokenId(EncryptedTokenValue encryptedTokenValue)
        : base(encryptedTokenValue)
        {
            var tokenValue = encryptedTokenValue.Decrypt();

            this.ApplicationId = tokenValue.ApplicationId;
            this.Type = tokenValue.Type;
            this.ExpiresAt = tokenValue.ExpiresAt;
            this.permissions = tokenValue.Permissions;
        }

        protected override void ValidateValue(EncryptedTokenValue value)
        {
        }

        public static TokenId GenerateAccessTokenId(
            ApplicationId applicationId,
            IEnumerable<PermissionId> permissions)
        {
            TokenValue tokenValue = TokenValue.GenerateAccessToken(applicationId, permissions);

            return new TokenId(EncryptedTokenValue.Encrypt(tokenValue));
        }

        public static TokenId GenerateRefreshTokenId(
            ApplicationId applicationId,
            IEnumerable<PermissionId> permissions,
            DateTime? expiresAt = null)
        {
            TokenValue tokenValue = TokenValue.GenerateRefreshToken(applicationId, permissions, expiresAt);

            return new TokenId(EncryptedTokenValue.Encrypt(tokenValue));
        }

        public override string ToString()
            => this.Value.ToString();

        public TokenValue Decrypt()
            => this.Value.Decrypt();
    }
}