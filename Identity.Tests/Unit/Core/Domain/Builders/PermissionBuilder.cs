using Identity.Core.Domain;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class PermissionBuilder
    {
        public static Permission DefaultPermission => new PermissionBuilder().Build();

        public PermissionId Id { get; private set; } = new(new ResourceId("MyResource"), "GrantPermission");
        public string Description { get; private set; } = "It allows user to grant permission to other users.";

        public PermissionBuilder WithId(PermissionId id)
        {
            this.Id = id;

            return this;
        }

        public PermissionBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public Permission Build()
            => new Permission(
                this.Id,
                this.Description);
    }
}