using System;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record UserPermission
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string PermissionResourceId { get; set; }
        public string PermissionName { get; set; }
    }
}