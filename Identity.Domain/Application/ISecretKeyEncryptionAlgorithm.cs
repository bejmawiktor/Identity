namespace Identity.Domain
{
    public interface ISecretKeyEncryptionAlgorithm
    {
        byte[] Encrypt(SecretKey secretKey);

        SecretKey Decrypt(byte[] encryptedSecretKey);

        void Validate(byte[] encryptedSecretKey);
    }
}