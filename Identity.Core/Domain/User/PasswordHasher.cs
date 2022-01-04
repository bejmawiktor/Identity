using System;

namespace Identity.Core.Domain
{
    internal static class PasswordHasher
    {
        public static int AlgorithmSymbolLength => 1;
        public static IPasswordHashingAlgorithm CurrentAlgorithm => new Pbkdf2HashingAlgorithm();

        public static HashedPassword Hash(Password password)
        {
            if(password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] algorithmHashedPassword = CurrentAlgorithm.Hash(password);

            return new HashedPassword(AssemblyHashedPassword(algorithmHashedPassword));
        }

        private static byte[] AssemblyHashedPassword(byte[] algorithmHashedPassword)
        {
            byte[] hashedPassword = new byte[AlgorithmSymbolLength + algorithmHashedPassword.Length];

            hashedPassword[0] = PasswordHashingAlgorithmFactory.ConvertToAlgorithmSymbol(CurrentAlgorithm.GetType());
            Buffer.BlockCopy(algorithmHashedPassword, 0, hashedPassword, AlgorithmSymbolLength, algorithmHashedPassword.Length);

            return hashedPassword;
        }

        public static PasswordVerificationResult Verify(
            HashedPassword hashedPassword,
            Password verifiedPassword)
        {
            if(verifiedPassword == null)
            {
                throw new ArgumentNullException(nameof(verifiedPassword));
            }

            if(hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            byte[] hashedPasswordBytes = hashedPassword.ToByteArray();
            byte algorithmSymbol = ExtractAlgorithmSymbol(hashedPasswordBytes);
            IPasswordHashingAlgorithm passwordHashingAlgorithm = PasswordHashingAlgorithmFactory.Create(
                algorithmSymbol);

            return passwordHashingAlgorithm.Verify(ExtractAlgorithmPassword(hashedPasswordBytes), verifiedPassword);
        }

        private static byte ExtractAlgorithmSymbol(byte[] hashedPasswordBytes)
            => hashedPasswordBytes[0];

        private static byte[] ExtractAlgorithmPassword(byte[] hashedPasswordBytes)
        {
            byte[] algorithmHashedPassword = new byte[hashedPasswordBytes.Length - AlgorithmSymbolLength];

            Buffer.BlockCopy(hashedPasswordBytes, AlgorithmSymbolLength, algorithmHashedPassword, 0, algorithmHashedPassword.Length);

            return algorithmHashedPassword;
        }

        public static void Validate(byte[] hashedPassword)
        {
            if(hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if(hashedPassword.Length == 0)
            {
                throw new ArgumentException("Incorrect hashed password given.");
            }

            byte algorithmSymbol = ExtractAlgorithmSymbol(hashedPassword);
            IPasswordHashingAlgorithm passwordHashingAlgorithm = PasswordHashingAlgorithmFactory.Create(
                algorithmSymbol);

            passwordHashingAlgorithm.Validate(ExtractAlgorithmPassword(hashedPassword));
        }
    }
}