using System;

namespace Identity.Core.Domain
{
    internal static class TokenValueEncrypter
    {
        public static int AlgorithmSymbolLength => 1;
        public static ITokenValueEncryptionAlgorithm CurrentAlgorithm => new AESTokenValueEncryptionAlgorithm();

        public static EncryptedTokenValue Encrypt(TokenValue tokenValue)
        {
            if(tokenValue == null)
            {
                throw new ArgumentNullException(nameof(tokenValue));
            }

            var encryptedTokenValue = CurrentAlgorithm.Encrypt(tokenValue);

            return new EncryptedTokenValue(AssemblyEncryptedTokenValue(encryptedTokenValue));
        }

        private static byte[] AssemblyEncryptedTokenValue(byte[] algorithmEncryptedTokenValue)
        {
            var encryptedTokenValue = new byte[AlgorithmSymbolLength + algorithmEncryptedTokenValue.Length];

            encryptedTokenValue[0] = TokenValueEncryptionAlgorithmFactory.ConvertToAlgorithmSymbol(CurrentAlgorithm.GetType());
            Buffer.BlockCopy(algorithmEncryptedTokenValue, 0, encryptedTokenValue, AlgorithmSymbolLength, algorithmEncryptedTokenValue.Length);

            return encryptedTokenValue;
        }

        public static TokenValue Decrypt(EncryptedTokenValue encryptedTokenValue)
        {
            if(encryptedTokenValue == null)
            {
                throw new ArgumentNullException(nameof(encryptedTokenValue));
            }

            byte[] encryptedTokenValueBytes = encryptedTokenValue.ToByteArray();
            byte algorithmSymbol = ExtractAlgorithmSymbol(encryptedTokenValueBytes);
            ITokenValueEncryptionAlgorithm tokenValueEncryptionAlgorithm = TokenValueEncryptionAlgorithmFactory.Create(
                algorithmSymbol);

            return tokenValueEncryptionAlgorithm.Decrypt(ExtractAlgorithmTokenValue(encryptedTokenValueBytes));
        }

        private static byte ExtractAlgorithmSymbol(byte[] encryptedTokenValueBytes)
            => encryptedTokenValueBytes[0];

        private static byte[] ExtractAlgorithmTokenValue(byte[] encryptedTokenValueBytes)
        {
            var encryptedTokenValue = new byte[encryptedTokenValueBytes.Length - AlgorithmSymbolLength];

            Buffer.BlockCopy(encryptedTokenValueBytes, AlgorithmSymbolLength, encryptedTokenValue, 0, encryptedTokenValue.Length);

            return encryptedTokenValue;
        }

        public static void Validate(byte[] encryptedTokenValue)
        {
            if(encryptedTokenValue == null)
            {
                throw new ArgumentNullException(nameof(encryptedTokenValue));
            }

            if(encryptedTokenValue.Length == 0)
            {
                throw new ArgumentException("Incorrect encrypted token value given.");
            }

            byte algorithmSymbol = ExtractAlgorithmSymbol(encryptedTokenValue);
            ITokenValueEncryptionAlgorithm tokenValueEncryptionAlgorithm = TokenValueEncryptionAlgorithmFactory.Create(
                algorithmSymbol);

            tokenValueEncryptionAlgorithm.Validate(ExtractAlgorithmTokenValue(encryptedTokenValue));
        }
    }
}