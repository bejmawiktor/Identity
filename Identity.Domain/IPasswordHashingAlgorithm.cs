namespace Identity.Domain
{
    internal interface IPasswordHashingAlgorithm
    {
        byte[] Hash(string password);

        PasswordVerificationResult Verify(byte[] hashedPassword, string verifiedPassword);

        void Validate(byte[] hashedPassword);
    }
}