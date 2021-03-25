using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Persistence.MSSQL
{
    internal record RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RolePermissionDto> Permissions { get; set; }

        public RoleDto(Identity.Application.RoleDto roleDto)
        {
            this.Id = roleDto.Id;
            this.Name = roleDto.Name;
            this.Description = roleDto.Description;
            this.Permissions = roleDto.Permissions.Select(r =>
                new RolePermissionDto()
                {
                    PermissionResourceId = r.ResourceId,
                    PermissionName = r.Name,
                    RoleId = roleDto.Id,
                    RoleDto = this
                }).ToList();
        }

        public RoleDto()
        {
        }

        public Identity.Application.RoleDto ToApplicationDto()
            => new Identity.Application.RoleDto(
                this.Id,
                this.Name,
                this.Description,
                this.Permissions
                    .Select(p => (p.PermissionResourceId, p.PermissionName)));
    }
}