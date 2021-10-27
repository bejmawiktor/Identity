using Identity.Application;
using System;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record AuthorizationCode
    {
        public string Code { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }

        public AuthorizationCode(AuthorizationCodeDto authorizationCode)
        {
            this.SetFields(authorizationCode);
        }

        public void SetFields(AuthorizationCodeDto authorizationCode)
        {
            this.Code = authorizationCode.Code;
            this.ApplicationId = authorizationCode.ApplicationId;
            this.ExpiresAt = authorizationCode.ExpiresAt;
            this.Used = authorizationCode.Used;
        }

        public AuthorizationCode()
        {
        }

        public AuthorizationCodeDto ToDto()
            => new AuthorizationCodeDto(this.Code, this.ApplicationId, this.ExpiresAt, this.Used);
    }
}
