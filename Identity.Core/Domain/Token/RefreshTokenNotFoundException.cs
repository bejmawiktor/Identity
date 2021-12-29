using System;

namespace Identity.Core.Domain
{
    public class RefreshTokenNotFoundException : Exception
    {
        public RefreshTokenNotFoundException() : base()
        {
        }

        internal RefreshTokenNotFoundException(TokenId tokenId)
        : base($"Refresh token {tokenId} not found.")
        {
        }

        public RefreshTokenNotFoundException(string message) : base(message)
        {
        }

        public RefreshTokenNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}