using System;

namespace Identity.Core.Domain
{
    internal static class TokenValueEncryptionAlgorithmFactory
    {
        public static ITokenValueEncryptionAlgorithm Create(byte algorithmSymbol)
        {
            switch(algorithmSymbol)
            {
                case 1:
                    return new AESTokenValueEncryptionAlgorithm();

                default:
                    throw new UnknownTokenValueEncryptionAlgorithmException("Unrecognized algorithm symbol given.");
            }
        }

        public static byte ConvertToAlgorithmSymbol(Type algorithmType)
        {
            switch(algorithmType.Name)
            {
                case nameof(AESTokenValueEncryptionAlgorithm):
                    return 1;

                default:
                    throw new UnknownTokenValueEncryptionAlgorithmException("Unrecognized algorithm type given.");
            };
        }
    }
}