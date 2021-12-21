using System;

namespace Identity.Core.Domain
{
    internal static class TokenValueEncoder
    {
        public static ITokenValueEncodingAlgorithm CurrentAlgorithm => new HS256JWTTokenValueEncodingAlgorithm();

        public static TokenValue Encode(TokenInformation tokenInformation)
        {
            if(tokenInformation == null)
            {
                throw new ArgumentNullException(nameof(tokenInformation));
            }

            var encodedTokenValue = CurrentAlgorithm.Encode(tokenInformation);

            return new TokenValue(AssemblyEncodedTokenValue(encodedTokenValue));
        }

        private static string AssemblyEncodedTokenValue(string algorithmEncodedTokenValue)
            => $"{TokenValueEncodingAlgorithmFactory.ConvertToAlgorithmSymbol(CurrentAlgorithm.GetType())}.{algorithmEncodedTokenValue}";

        public static TokenInformation Decode(TokenValue tokenValue)
        {
            if(tokenValue == null)
            {
                throw new ArgumentNullException(nameof(tokenValue));
            }

            byte algorithmSymbol = ExtractAlgorithmSymbol(tokenValue.ToString());
            ITokenValueEncodingAlgorithm tokenValueEncodingAlgorithm = TokenValueEncodingAlgorithmFactory.Create(
                algorithmSymbol);

            return tokenValueEncodingAlgorithm.Decode(ExtractAlgorithmTokenValue(tokenValue.ToString()));
        }

        private static byte ExtractAlgorithmSymbol(string encodedTokenValue)
        {
            string[] tokenParts = encodedTokenValue.Split('.');
            byte.TryParse(tokenParts[0], out byte symbol);

            return symbol;
        }

        private static string ExtractAlgorithmTokenValue(string encodedTokenValue)
        {
            return encodedTokenValue.Substring(encodedTokenValue.IndexOf('.') + 1);
        }

        public static void Validate(string tokenValue)
        {
            if(tokenValue == null)
            {
                throw new ArgumentNullException(nameof(tokenValue));
            }

            if(tokenValue.Length == 0)
            {
                throw new ArgumentException("Incorrect token value given.");
            }

            byte algorithmSymbol = ExtractAlgorithmSymbol(tokenValue);
            ITokenValueEncodingAlgorithm tokenValueEncodingAlgorithm = TokenValueEncodingAlgorithmFactory.Create(
                algorithmSymbol);

            tokenValueEncodingAlgorithm.Validate(ExtractAlgorithmTokenValue(tokenValue));
        }
    }
}
