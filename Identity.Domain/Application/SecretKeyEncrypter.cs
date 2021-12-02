using System;

namespace Identity.Domain
{
    internal static class SecretKeyEncrypter
    {
        public static int AlgorithmSymbolLength => 1;
        public static ISecretKeyEncryptionAlgorithm CurrentAlgorithm => new AESSecretKeyEncryptionAlgorithm();

        public static EncryptedSecretKey Encrypt(SecretKey secretKey)
        {
            if(secretKey == null)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            var encryptedSecretKey = CurrentAlgorithm.Encrypt(secretKey);

            return new EncryptedSecretKey(AssemblyEncryptedSecretKey(encryptedSecretKey));
        }

        private static byte[] AssemblyEncryptedSecretKey(byte[] algorithmEncryptedSecretKey)
        {
            var encryptedSecretKey = new byte[AlgorithmSymbolLength + algorithmEncryptedSecretKey.Length];

            encryptedSecretKey[0] = SecretKeyEncryptionAlgorithmFactory.ConvertToAlgorithmSymbol(CurrentAlgorithm.GetType());
            Buffer.BlockCopy(algorithmEncryptedSecretKey, 0, encryptedSecretKey, AlgorithmSymbolLength, algorithmEncryptedSecretKey.Length);

            return encryptedSecretKey;
        }

        public static SecretKey Decrypt(EncryptedSecretKey encryptedSecretKey)
        {
            if(encryptedSecretKey == null)
            {
                throw new ArgumentNullException(nameof(encryptedSecretKey));
            }

            byte[] encryptedSecretKeyBytes = encryptedSecretKey.ToByteArray();
            byte algorithmSymbol = ExtractAlgorithmSymbol(encryptedSecretKeyBytes);
            ISecretKeyEncryptionAlgorithm secretKeyEncryptionAlgorithm = SecretKeyEncryptionAlgorithmFactory.Create(
                algorithmSymbol);

            return secretKeyEncryptionAlgorithm.Decrypt(ExtractAlgorithmSecretKey(encryptedSecretKeyBytes));
        }

        private static byte ExtractAlgorithmSymbol(byte[] encryptedSecretKeyBytes)
            => encryptedSecretKeyBytes[0];

        private static byte[] ExtractAlgorithmSecretKey(byte[] encryptedSecretKeyBytes)
        {
            var encryptedSecretKey = new byte[encryptedSecretKeyBytes.Length - AlgorithmSymbolLength];

            Buffer.BlockCopy(encryptedSecretKeyBytes, AlgorithmSymbolLength, encryptedSecretKey, 0, encryptedSecretKey.Length);

            return encryptedSecretKey;
        }

        public static void Validate(byte[] encryptedSecretKey)
        {
            if(encryptedSecretKey == null)
            {
                throw new ArgumentNullException(nameof(encryptedSecretKey));
            }

            if(encryptedSecretKey.Length == 0)
            {
                throw new ArgumentException("Incorrect encrypted secret key given.");
            }

            byte algorithmSymbol = ExtractAlgorithmSymbol(encryptedSecretKey);
            ISecretKeyEncryptionAlgorithm secretKeyEncryptionAlgorithm = SecretKeyEncryptionAlgorithmFactory.Create(
                algorithmSymbol);

            secretKeyEncryptionAlgorithm.Validate(ExtractAlgorithmSecretKey(encryptedSecretKey));
        }
    }
}