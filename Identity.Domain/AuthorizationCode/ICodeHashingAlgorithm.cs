namespace Identity.Domain
{
    internal interface ICodeHashingAlgorithm
    {
        byte[] Hash(Code code);

        void Validate(byte[] hashedCode);
    }
}
