namespace Identity.Domain
{
    internal interface ITokenValueEncodingAlgorithm
    {
        string Encode(TokenInformation tokenInformation);

        TokenInformation Decode(string token);

        void Validate(string token);
    }
}