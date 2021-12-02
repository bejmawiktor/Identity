using Identity.Domain;

namespace Identity.Tests.Unit.Domain
{
    public class TokenStub : Token
    {
        private bool Used { get; }

        public TokenStub(TokenId id, bool used = false) : base(id)
        {
            this.Used = used;
        }

        protected override TokenVerificationResult GetTokenExtraVerification() 
        { 
            if(this.Used)
            {
                return TokenVerificationResult.Failed.WithMessage("Token has been used.");
            }

            return TokenVerificationResult.Success; 
        }
    }
}
