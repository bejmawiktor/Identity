using DDD.Application.Model.Converters;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    internal class RefreshTokenDtoConverter : IAggregateRootDtoConverter<RefreshToken, TokenId, RefreshTokenDto, string>
    {
        public RefreshTokenDto ToDto(RefreshToken refreshToken)
        {
            if(refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            return new RefreshTokenDto(refreshToken.Id.ToString(), refreshToken.Used);
        }

        public string ToDtoIdentifier(TokenId tokenId)
        {
            if(tokenId == null)
            {
                throw new ArgumentNullException(nameof(tokenId));
            }

            return tokenId.ToString();
        }
    }
}