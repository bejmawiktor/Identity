namespace Identity.Domain
{
    internal interface IPasswordHashingAlgorithm
    {
        byte[] Hash(Password password);

        PasswordVerificationResult Verify(byte[] hashedPassword, Password verifiedPassword);

        void Validate(byte[] hashedPassword);
    }
}