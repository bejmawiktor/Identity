namespace Identity.Core.Domain
{
    internal interface ISecretKeyEncryptionAlgorithm
    {
        byte[] Encrypt(SecretKey secretKey);

        SecretKey Decrypt(byte[] encryptedSecretKey);

        void Validate(byte[] encryptedSecretKey);
    }
}