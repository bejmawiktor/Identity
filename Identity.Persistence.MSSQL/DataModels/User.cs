using Identity.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public IEnumerable<UserRole> Roles { get; set; }
        public IEnumerable<UserPermission> Permissions { get; set; }

        public User(UserDto userDto)
        {
            this.SetFields(userDto);
        }

        public User()
        {
        }

        public void SetFields(UserDto userDto)
        {
            this.Id = userDto.Id;
            this.Email = userDto.Email;
            this.HashedPassword = userDto.HashedPassword;
            this.Roles = userDto.Roles.Select(r =>
                new UserRole()
                {
                    RoleId = r,
                    UserId = userDto.Id,
                    User = this
                }).ToList();
            this.Permissions = userDto.Permissions.Select(p =>
                new UserPermission()
                {
                    PermissionResourceId = p.ResourceId,
                    PermissionName = p.Name,
                    UserId = userDto.Id,
                    User = this
                }).ToList();
        }

        public UserDto ToDto()
            => new UserDto(
                this.Id,
                this.Email,
                this.HashedPassword,
                this.Roles.Select(r => r.RoleId),
                this.Permissions.Select(p => (p.PermissionResourceId, p.PermissionName)));
    }
}