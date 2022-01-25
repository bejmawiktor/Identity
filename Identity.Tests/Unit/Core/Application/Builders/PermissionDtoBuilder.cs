using Identity.Core.Application;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    internal class PermissionDtoBuilder
    {
        public static PermissionDto DefaultPermissionDto => new PermissionDtoBuilder().Build();

        public string ResourceId { get; private set; } = "MyResource";
        public string Name { get; private set; } = "GrantPermission";
        public string Description { get; private set; } = "It allows user to grant permission to other users.";

        public PermissionDtoBuilder WithResourceId(string resourceId)
        {
            this.ResourceId = resourceId;

            return this;
        }

        public PermissionDtoBuilder WithName(string name)
        {
            this.Name = name;

            return this;
        }

        public PermissionDtoBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public PermissionDto Build()
            => new PermissionDto(
                this.ResourceId,
                this.Name,
                this.Description);
    }
}