using System;

namespace Identity.Core.Domain
{
    internal class RefreshToken : Token
    {
        public bool Used { get; private set; }

        public RefreshToken(TokenId id, bool used = false) : base(id)
        {
            this.ValidateId(id);

            this.Used = used;
        }

        private void ValidateId(TokenId id)
        {
            if(id.Type == TokenType.Access)
            {
                throw new ArgumentException("Access token id given.");
            }
        }

        protected override TokenVerificationResult GetTokenExtraVerification()
        {
            if(this.Used)
            {
                return TokenVerificationResult.Failed.WithMessage("Token was used before.");
            }

            return TokenVerificationResult.Success;
        }

        public void Use()
        {
            TokenVerificationResult verificationResult = this.Verify();

            if(verificationResult == TokenVerificationResult.Failed)
            {
                throw new InvalidOperationException(verificationResult.Message);
            }

            this.Used = true;
        }
    }
}