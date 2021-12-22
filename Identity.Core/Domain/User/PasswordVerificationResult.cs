using DDD.Domain.Model;

namespace Identity.Core.Domain
{
    internal class PasswordVerificationResult : Enumeration<string, PasswordVerificationResult>
    {
        public static readonly PasswordVerificationResult Success = new PasswordVerificationResult(nameof(Success));
        public static readonly PasswordVerificationResult Failed = new PasswordVerificationResult(nameof(Failed));

        protected override string DefaultValue => nameof(Failed);

        public PasswordVerificationResult()
        {
        }

        protected PasswordVerificationResult(string value) : base(value)
        {
        }
    }
}