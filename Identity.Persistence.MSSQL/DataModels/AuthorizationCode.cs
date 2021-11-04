using Identity.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record AuthorizationCode
    {
        public string Code { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }
        public IEnumerable<AuthorizationCodePermission> Permissions { get; set; }

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
            this.Permissions = authorizationCode.Permissions.Select(p =>
                new AuthorizationCodePermission()
                {
                    PermissionResourceId = p.ResourceId,
                    PermissionName = p.Name,
                    ApplicationId = authorizationCode.ApplicationId,
                    Code = authorizationCode.Code,
                    AuthorizationCode = this
                }).ToList();
        }

        public AuthorizationCode()
        {
        }

        public AuthorizationCodeDto ToDto()
            => new AuthorizationCodeDto(
                this.Code,
                this.ApplicationId,
                this.ExpiresAt,
                this.Used,
                this.Permissions.Select(p => (p.PermissionResourceId, p.PermissionName)));
    }
}