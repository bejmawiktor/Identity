using Identity.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RolePermission> Permissions { get; set; }

        public Role(RoleDto roleDto)
        {
            this.Id = roleDto.Id;
            this.Name = roleDto.Name;
            this.Description = roleDto.Description;
            this.Permissions = roleDto.Permissions.Select(r =>
                new RolePermission()
                {
                    PermissionResourceId = r.ResourceId,
                    PermissionName = r.Name,
                    RoleId = roleDto.Id,
                    Role = this
                }).ToList();
        }

        public Role()
        {
        }

        public RoleDto ToDto()
            => new RoleDto(
                this.Id,
                this.Name,
                this.Description,
                this.Permissions
                    .Select(p => (p.PermissionResourceId, p.PermissionName)));
    }
}