using Identity.Core.Application;
using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    internal class RoleDtoBuilder
    {
        public static readonly Guid DefaultId = Guid.NewGuid();

        public static readonly IEnumerable<PermissionId> DefaultPermissions = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource"), "MyPermission"),
            new PermissionId(new ResourceId("MyResource2"), "MyPermission2")
        };

        public static RoleDto DefaultRoleDto => new RoleDtoBuilder().Build();

        public Guid Id { get; private set; } = RoleDtoBuilder.DefaultId;
        public string Name { get; private set; } = "RoleName";
        public string Description { get; private set; } = "Test role description";

        public IEnumerable<(string ResourceId, string Name)> Permissions { get; private set; } = new (string ResourceId, string Name)[]
        {
            ("MyResource", "MyPermission"),
            ("MyResource2", "MyPermission2")
        };

        public RoleDtoBuilder WithId(Guid id)
        {
            this.Id = id;

            return this;
        }

        public RoleDtoBuilder WithName(string name)
        {
            this.Name = name;

            return this;
        }

        public RoleDtoBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public RoleDtoBuilder WithPermissions(IEnumerable<(string ResourceId, string Name)> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public RoleDto Build()
            => new RoleDto(
                this.Id,
                this.Name,
                this.Description,
                this.Permissions);
    }
}