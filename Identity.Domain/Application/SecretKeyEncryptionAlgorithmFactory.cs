using System;

namespace Identity.Domain
{
    internal static class SecretKeyEncryptionAlgorithmFactory
    {
        public static ISecretKeyEncryptionAlgorithm Create(byte algorithmSymbol)
        {
            switch(algorithmSymbol)
            {
                case 1:
                    return new AESSecretKeyEncryptionAlgorithm();

                default:
                    throw new UnknownSecretKeyEncryptionAlgorithmException("Unrecognized algorithm symbol given.");
            }
        }

        public static byte ConvertToAlgorithmSymbol(Type algorithmType)
        {
            switch(algorithmType.Name)
            {
                case nameof(AESSecretKeyEncryptionAlgorithm):
                    return 1;

                default:
                    throw new UnknownSecretKeyEncryptionAlgorithmException("Unrecognized algorithm type given.");
            };
        }
    }
}