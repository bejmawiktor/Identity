namespace Identity.Domain
{
    internal interface ITokenGenerationAlgorithm
    {
        string Encode(TokenInformation tokenInformation);

        TokenInformation Decode(string token);

        void Validate(string token);
    }
}