using System;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record AuthorizationCodePermission
    {
        public string Code { get; set; }
        public Guid ApplicationId { get; set; }
        public AuthorizationCode AuthorizationCode { get; set; }
        public string PermissionResourceId { get; set; }
        public string PermissionName { get; set; }
    }
}