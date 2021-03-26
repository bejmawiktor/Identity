using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Persistence.MSSQL
{
    internal record UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public IEnumerable<UserRoleDto> Roles { get; set; }
        public IEnumerable<UserPermissionDto> Permissions { get; set; }

        public UserDto(Identity.Application.UserDto userDto)
        {
            this.Id = userDto.Id;
            this.Email = userDto.Email;
            this.HashedPassword = userDto.HashedPassword;
            this.Roles = userDto.Roles.Select(r =>
                new UserRoleDto()
                {
                    RoleId = r,
                    UserId = userDto.Id,
                    UserDto = this
                }).ToList();
            this.Permissions = userDto.Permissions.Select(p =>
                new UserPermissionDto()
                {
                    PermissionResourceId = p.ResourceId,
                    PermissionName = p.Name,
                    UserId = userDto.Id,
                    UserDto = this
                }).ToList();
        }

        public UserDto()
        {
        }

        public Identity.Application.UserDto ToApplicationDto()
            => new Application.UserDto(
                this.Id, 
                this.Email, 
                this.HashedPassword,
                this.Roles.Select(r => r.RoleId),
                this.Permissions.Select(p => (p.PermissionResourceId, p.PermissionName)));
    }
}