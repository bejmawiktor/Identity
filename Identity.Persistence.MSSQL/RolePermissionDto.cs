using System;

namespace Identity.Persistence.MSSQL
{
    internal record RolePermissionDto
    {
        public Guid RoleId { get; set; }
        public RoleDto RoleDto { get; set; }
        public string PermissionResourceId { get; set; }
        public string PermissionName { get; set; }
    }
}