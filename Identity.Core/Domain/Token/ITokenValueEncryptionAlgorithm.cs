namespace Identity.Core.Domain
{
    internal interface ITokenValueEncryptionAlgorithm
    {
        byte[] Encrypt(TokenValue tokenValue);

        void Validate(byte[] encryptedTokenValue);

        TokenValue Decrypt(byte[] encryptedTokenValue);
    }
}