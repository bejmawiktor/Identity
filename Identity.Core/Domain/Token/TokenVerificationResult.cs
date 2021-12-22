using DDD.Domain.Model;

namespace Identity.Core.Domain
{
    internal class TokenVerificationResult : Enumeration<string, TokenVerificationResult>
    {
        public static readonly TokenVerificationResult Success = new TokenVerificationResult(nameof(Success));
        public static readonly TokenVerificationResult Failed = new TokenVerificationResult(nameof(Failed));

        public string Message { get; private set; }

        protected override string DefaultValue => nameof(Failed);

        public TokenVerificationResult()
        {
            this.Message = string.Empty;
        }

        private TokenVerificationResult(string value, string message) : base(value)
        {
            this.Message = message ?? string.Empty;
        }

        protected TokenVerificationResult(string value) : base(value)
        {
        }

        internal TokenVerificationResult WithMessage(string message)
            => new TokenVerificationResult(this.Value, message);
    }
}