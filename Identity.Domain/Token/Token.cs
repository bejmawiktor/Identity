using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    public abstract class Token : AggregateRoot<TokenId>
    {
        public ApplicationId ApplicationId => this.Id.ApplicationId;
        public TokenType Type => this.Id.Type;
        public DateTime ExpiresAt => this.Id.ExpiresAt;
        public IReadOnlyCollection<PermissionId> Permissions => this.Id.Permissions;

        public Token(TokenId id) : base(id)
        {
        }

        public TokenVerificationResult Verify()
        {
            TokenVerificationResult extraVerificationResult = this.GetTokenExtraVerification();

            if(extraVerificationResult != TokenVerificationResult.Success)
            {
                return extraVerificationResult;
            }

            if(this.Id.Expired)
            {
                return TokenVerificationResult.Failed.WithMessage("Token has expired.");
            }

            return TokenVerificationResult.Success;
        }

        protected virtual TokenVerificationResult GetTokenExtraVerification()
            => TokenVerificationResult.Success;

        public override string ToString() 
            => this.Id.ToString();
    }
}
