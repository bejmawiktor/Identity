using System;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public string PermissionResourceId { get; set; }
        public string PermissionName { get; set; }
    }
}