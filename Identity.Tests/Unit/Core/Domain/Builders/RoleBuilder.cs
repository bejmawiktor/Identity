using Identity.Core.Domain;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class RoleBuilder
    {
        public static readonly RoleId DefaultId = RoleId.Generate();
        public static Role DefaultRole => new RoleBuilder().Build();

        public RoleId Id { get; private set; } = RoleBuilder.DefaultId;
        public string Name { get; private set; } = "RoleName";
        public string Description { get; private set; } = "Test role description";
        public IEnumerable<PermissionId> Permissions { get; private set; }

        public RoleBuilder WithId(RoleId id)
        {
            this.Id = id;

            return this;
        }

        public RoleBuilder WithName(string name)
        {
            this.Name = name;

            return this;
        }

        public RoleBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public RoleBuilder WithPermissions(IEnumerable<PermissionId> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public Role Build()
            => new Role(
                this.Id,
                this.Name,
                this.Description,
                this.Permissions);
    }
}