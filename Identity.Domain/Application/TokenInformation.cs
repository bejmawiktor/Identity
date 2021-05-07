using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    internal class TokenInformation : ValueObject
    {
        public ApplicationId ApplicationId { get; }
        public TokenType TokenType { get; }
        public DateTime ExpirationDate { get; }

        public TokenInformation(
            ApplicationId applicationId,
            TokenType tokenType,
            DateTime? expirationDate = null)
        {
            this.TokenType = tokenType;
            this.ApplicationId = applicationId;
            this.ExpirationDate = expirationDate ?? tokenType.GenerateExpirationDate();
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.ApplicationId;
            yield return this.TokenType;
            yield return this.ExpirationDate;
        }
    }
}