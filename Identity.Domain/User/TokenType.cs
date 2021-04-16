using DDD.Domain.Model;
using System;

namespace Identity.Domain
{
    public sealed class TokenType : Enumeration<string, TokenType>
    {
        public static readonly TokenType Access = new TokenType(nameof(Access), () => DateTime.Now.AddDays(1));
        public static readonly TokenType Refresh = new TokenType(nameof(Refresh), () => DateTime.Now.AddYears(1));

        public string Name => this.Value;
        private Func<DateTime> GenerateExpirationDateFunc { get; }

        protected override string DefaultValue => Access.Value;
        private Func<DateTime> DefaultGenerateExpirationDateFunc => Access.GenerateExpirationDateFunc;

        public TokenType() : base()
        {
            this.GenerateExpirationDateFunc = DefaultGenerateExpirationDateFunc;
        }

        private TokenType(string name, Func<DateTime> generateExpirationDateFunc) : base(name)
        {
            this.GenerateExpirationDateFunc = generateExpirationDateFunc;
        }

        internal DateTime GenerateExpirationDate()
            => this.GenerateExpirationDateFunc.Invoke();

        internal static TokenType FromName(string name)
        {
            switch(name)
            {
                case nameof(TokenType.Access):
                    return TokenType.Access;

                case nameof(TokenType.Refresh):
                    return TokenType.Refresh;

                default:
                    throw new InvalidTokenException("Invalid token type given.");
            }
        }
    }
}