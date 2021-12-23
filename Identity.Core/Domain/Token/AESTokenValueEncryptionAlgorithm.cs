using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Core.Domain
{
    internal class AESTokenValueEncryptionAlgorithm : ITokenValueEncryptionAlgorithm
    {
        private static readonly int AesBlockByteSize = 128 / 8;
        private static readonly string Key = "gDZ2Z4VUbtunNuhtCSdxMfiGcjbbcaWGhKS6UwP9DV6fTcBSw58HqGMUaY8APHq6";
        private static readonly byte[] Iv 
            = new byte[] { 10, 58, 188, 94, 18, 176, 128, 190, 13, 128, 126, 166, 42, 11, 100, 175 };

        public TokenValue Decrypt(byte[] encryptedTokenValue)
        {
            this.Validate(encryptedTokenValue);

            byte[] key = this.GetKey(Key);

            using(var aes = Aes.Create())
            {
                byte[] iv = encryptedTokenValue.Take(AesBlockByteSize).ToArray();
                byte[] cipherText = encryptedTokenValue.Skip(AesBlockByteSize).ToArray();

                using(var decryptor = aes.CreateDecryptor(key, iv))
                {
                    byte[] decryptedBytes = decryptor
                        .TransformFinalBlock(cipherText, 0, cipherText.Length);

                    return new TokenValue(Encoding.UTF8.GetString(decryptedBytes));
                }
            }
        }

        public byte[] Encrypt(TokenValue tokenValue)
        {
            if(tokenValue == null)
            {
                throw new ArgumentNullException(nameof(tokenValue));
            }

            byte[] key = this.GetKey(Key);

            using(Aes aes = Aes.Create())
            {
                using(ICryptoTransform encryptor = aes.CreateEncryptor(key, AESTokenValueEncryptionAlgorithm.Iv))
                {
                    byte[] plainText = Encoding.UTF8.GetBytes(tokenValue.ToString());
                    byte[] cipherText = encryptor
                        .TransformFinalBlock(plainText, 0, plainText.Length);
                    var result = new byte[AESTokenValueEncryptionAlgorithm.Iv.Length + cipherText.Length];

                    AESTokenValueEncryptionAlgorithm.Iv.CopyTo(result, 0);
                    cipherText.CopyTo(result, AESTokenValueEncryptionAlgorithm.Iv.Length);

                    return result;
                }
            }
        }

        private byte[] GetKey(string tokenValue)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(tokenValue);

            using(MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(keyBytes);
            }
        }

        public void Validate(byte[] encryptedTokenValue)
        {
            if(encryptedTokenValue == null)
            {
                throw new ArgumentNullException(nameof(encryptedTokenValue));
            }

            if(encryptedTokenValue == Array.Empty<byte>())
            {
                throw new ArgumentException("Encrypted token value can't be empty.");
            }

            if(encryptedTokenValue.Length % AesBlockByteSize != 0)
            {
                throw new ArgumentException("Wrong encrypted token value given.");
            }
        }
    }
}