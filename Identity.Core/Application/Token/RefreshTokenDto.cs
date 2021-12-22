using DDD.Application.Model;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    public class RefreshTokenDto : IAggregateRootDto<RefreshToken, TokenId>
    {
        public string Id { get; set; }
        public bool Used { get; set; }

        public RefreshTokenDto(string id, bool used)
        {
            this.Id = id;
            this.Used = used;
        }

        public override bool Equals(object obj)
        {
            return obj is RefreshTokenDto dto
                && this.Id == dto.Id
                && this.Used == dto.Used;
        }

        public override int GetHashCode()
            => HashCode.Combine(this.Id, this.Used);

        internal RefreshToken ToRefreshToken()
            => new RefreshToken(
                id: new TokenId(new EncryptedTokenValue(this.Id)),
                used: this.Used);

        RefreshToken IDomainObjectDto<RefreshToken>.ToDomainObject()
            => this.ToRefreshToken();
    }
}