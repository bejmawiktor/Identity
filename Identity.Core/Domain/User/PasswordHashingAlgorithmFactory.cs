using System;

namespace Identity.Core.Domain
{
    internal static class PasswordHashingAlgorithmFactory
    {
        public static IPasswordHashingAlgorithm Create(byte algorithmSymbol)
        {
            switch(algorithmSymbol)
            {
                case 1:
                    return new Pbkdf2HashingAlgorithm();

                default:
                    throw new UnknownHashingPasswordAlgorithmException("Unrecognized algorithm symbol given.");
            }
        }

        public static byte ConvertToAlgorithmSymbol(Type algorithmType)
        {
            switch(algorithmType.Name)
            {
                case nameof(Pbkdf2HashingAlgorithm):
                    return 1;

                default:
                    throw new UnknownHashingPasswordAlgorithmException("Unrecognized algorithm type given.");
            };
        }
    }
}