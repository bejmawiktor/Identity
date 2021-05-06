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

            var algorithmHashedPassword = CurrentAlgorithm.Encrypt(secretKey);

            return new EncryptedSecretKey(AssemblyEncryptedSecretKey(algorithmHashedPassword));
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

            return secretKeyEncryptionAlgorithm.Decrypt(ExtractAlgorithmPassword(encryptedSecretKeyBytes));
        }

        private static byte ExtractAlgorithmSymbol(byte[] encryptedSecretKeyBytes)
            => encryptedSecretKeyBytes[0];

        private static byte[] ExtractAlgorithmPassword(byte[] encryptedSecretKeyBytes)
        {
            var algorithmHashedPassword = new byte[encryptedSecretKeyBytes.Length - AlgorithmSymbolLength];

            Buffer.BlockCopy(encryptedSecretKeyBytes, AlgorithmSymbolLength, algorithmHashedPassword, 0, algorithmHashedPassword.Length);

            return algorithmHashedPassword;
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

            secretKeyEncryptionAlgorithm.Validate(ExtractAlgorithmPassword(encryptedSecretKey));
        }
    }
}