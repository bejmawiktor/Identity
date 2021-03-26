using System;

namespace Identity.Persistence.MSSQL
{
    internal record UserPermissionDto
    {
        public Guid UserId { get; set; }
        public UserDto UserDto { get; set; }
        public string PermissionResourceId { get; set; }
        public string PermissionName { get; set; }
    }
}