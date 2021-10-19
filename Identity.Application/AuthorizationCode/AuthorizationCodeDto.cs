using DDD.Application.Model;
using Identity.Domain;
using System;

namespace Identity.Application
{
    using ApplicationId = Domain.ApplicationId;

    public class AuthorizationCodeDto : IAggregateRootDto<AuthorizationCode, AuthorizationCodeId>
    {
        public string Code { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }

        public AuthorizationCodeDto(
            string code,
            Guid applicationId,
            DateTime expiresAt,
            bool used)
        {
            this.Code = code;
            this.ApplicationId = applicationId;
            this.ExpiresAt = expiresAt;
            this.Used = used;
        }

        AuthorizationCode IDomainObjectDto<AuthorizationCode>.ToDomainObject()
            => this.ToAuthorizationCode();

        public AuthorizationCode ToAuthorizationCode()
            => new AuthorizationCode(
                new AuthorizationCodeId(this.Code, new ApplicationId(this.ApplicationId)),
                this.ExpiresAt,
                this.Used);

        public override bool Equals(object obj)
        {
            return obj is AuthorizationCodeDto dto
                && this.Code == dto.Code
                && this.ApplicationId.Equals(dto.ApplicationId)
                && this.ExpiresAt == dto.ExpiresAt
                && this.Used == dto.Used;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Code, this.ApplicationId, this.ExpiresAt, this.Used);
        }
    }
}
