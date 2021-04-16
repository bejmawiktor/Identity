using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    internal class TokenInformation : ValueObject
    {
        public UserId UserId { get; }
        public TokenType TokenType { get; }
        public DateTime ExpirationDate { get; }

        public TokenInformation(UserId userId, TokenType tokenType, DateTime? expirationDate = null)
        {
            this.UserId = userId;
            this.TokenType = tokenType;
            this.ExpirationDate = expirationDate ?? tokenType.GenerateExpirationDate();
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.UserId;
            yield return this.TokenType;
            yield return this.ExpirationDate;
        }
    }
}