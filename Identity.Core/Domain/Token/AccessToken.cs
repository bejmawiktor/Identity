using System;

namespace Identity.Core.Domain
{
    internal class AccessToken : Token
    {
        public AccessToken(TokenId id) : base(id)
        {
            this.ValidateId(id);
        }

        private void ValidateId(TokenId id)
        {
            if(id.Type == TokenType.Refresh)
            {
                throw new ArgumentException("Refresh token id given.");
            }
        }
    }
}