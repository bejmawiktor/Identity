using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Core.Domain
{
    internal class AESSecretKeyEncryptionAlgorithm : ISecretKeyEncryptionAlgorithm
    {
        private static readonly int AesBlockByteSize = 128 / 8;
        private static readonly string Key = "Z1cNu1DEHZ9QAhfn4W3PNoP0dXTtnEeXklziZs1vrLKZklY5hF6NxwoolKybaOoq";
        private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();

        public SecretKey Decrypt(byte[] encryptedSecretKey)
        {
            this.Validate(encryptedSecretKey);

            byte[] key = this.GetKey(Key);

            using(var aes = Aes.Create())
            {
                byte[] iv = encryptedSecretKey.Take(AesBlockByteSize).ToArray();
                byte[] cipherText = encryptedSecretKey.Skip(AesBlockByteSize).ToArray();

                using(var decryptor = aes.CreateDecryptor(key, iv))
                {
                    byte[] decryptedBytes = decryptor
                        .TransformFinalBlock(cipherText, 0, cipherText.Length);

                    return new SecretKey(Encoding.UTF8.GetString(decryptedBytes));
                }
            }
        }

        public byte[] Encrypt(SecretKey secretKey)
        {
            if(secretKey == null)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            byte[] key = this.GetKey(Key);

            using(Aes aes = Aes.Create())
            {
                byte[] iv = this.GenerateRandomBytes(AesBlockByteSize);

                using(ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
                {
                    byte[] plainText = Encoding.UTF8.GetBytes(secretKey.ToString());
                    byte[] cipherText = encryptor
                        .TransformFinalBlock(plainText, 0, plainText.Length);
                    var result = new byte[iv.Length + cipherText.Length];

                    iv.CopyTo(result, 0);
                    cipherText.CopyTo(result, iv.Length);

                    return result;
                }
            }
        }

        private byte[] GetKey(string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using(MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(keyBytes);
            }
        }

        private byte[] GenerateRandomBytes(int numberOfBytes)
        {
            byte[] randomBytes = new byte[numberOfBytes];

            Random.GetBytes(randomBytes);

            return randomBytes;
        }

        public void Validate(byte[] encryptedSecretKey)
        {
            if(encryptedSecretKey == null)
            {
                throw new ArgumentNullException(nameof(encryptedSecretKey));
            }

            if(encryptedSecretKey == Array.Empty<byte>())
            {
                throw new ArgumentException("Encrypted secret key can't be empty.");
            }

            if(encryptedSecretKey.Length % AesBlockByteSize != 0)
            {
                throw new ArgumentException("Wrong encrypted security key given.");
            }
        }
    }
}