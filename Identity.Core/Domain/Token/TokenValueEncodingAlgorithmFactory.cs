using System;

namespace Identity.Core.Domain
{
    internal static class TokenValueEncodingAlgorithmFactory
    {
        public static ITokenValueEncodingAlgorithm Create(byte symbol)
        {
            return symbol switch
            {
                1 => new HS256JWTTokenValueEncodingAlgorithm(),
                _ => throw new UnknownTokenValueEncodingAlgorithmException("Unrecognized algorithm symbol given.")
            };
        }

        public static byte ConvertToAlgorithmSymbol(Type algorithmType)
        {
            return algorithmType.Name switch
            {
                nameof(HS256JWTTokenValueEncodingAlgorithm) => 1,
                _ => throw new UnknownTokenValueEncodingAlgorithmException("Unrecognized algorithm type given.")
            };
        }
    }
}