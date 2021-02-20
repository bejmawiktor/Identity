using DDD.Model;

namespace Identity.Domain
{
    public class PasswordVerificationResult : Enumeration<string, PasswordVerificationResult>
    {
        public static PasswordVerificationResult Success => new PasswordVerificationResult(nameof(Success));
        public static PasswordVerificationResult Failed => new PasswordVerificationResult(nameof(Failed));

        protected override string DefaultValue => nameof(Failed);

        public PasswordVerificationResult()
        {
        }

        protected PasswordVerificationResult(string value) : base(value)
        {
        }
    }
}