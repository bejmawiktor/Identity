using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Core.Domain
{
    internal class TokenPair : ValueObject
    {
        public TokenValue AccessToken { get; }
        public TokenValue RefreshToken { get; }

        public TokenPair(TokenValue accessToken, TokenValue refreshToken)
        {
            this.ValidateMembers(accessToken, refreshToken);

            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }

        private void ValidateMembers(TokenValue accessToken, TokenValue refreshToken)
        {
            if(accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            if(refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if(accessToken.Type != TokenType.Access)
            {
                throw new ArgumentException("Wrong token type. Expected access token but got refresh instead.");
            }

            if(refreshToken.Type != TokenType.Refresh)
            {
                throw new ArgumentException("Wrong token type. Expected refresh token but got access instead.");
            }
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.AccessToken;
            yield return this.RefreshToken;
        }
    }
}