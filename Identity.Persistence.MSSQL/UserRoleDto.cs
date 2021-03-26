using System;

namespace Identity.Persistence.MSSQL
{
    internal record UserRoleDto
    {
        public Guid UserId { get; set; }
        public UserDto UserDto { get; set; }
        public Guid RoleId { get; set; }
    }
}