using System;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
    }
}