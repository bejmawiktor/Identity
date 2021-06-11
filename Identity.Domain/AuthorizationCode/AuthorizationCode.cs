using DDD.Domain.Model;
using System;

namespace Identity.Domain
{
    public class AuthorizationCode : AggregateRoot<AuthorizationCodeId>
    {
        public DateTime ExpiresAt { get; }
        public bool Used { get; }
        private int SecondsToExpire => 60;

        public AuthorizationCode(
            AuthorizationCodeId id,
            DateTime expiresAt,
            bool used)
        : base(id)
        {
            this.ExpiresAt = expiresAt;
            this.Used = used;
        }

        internal AuthorizationCode(
            AuthorizationCodeId id)
        : base(id)
        {
            this.ExpiresAt = DateTime.Now.AddSeconds(this.SecondsToExpire);
            this.Used = false;
        }
    }
}