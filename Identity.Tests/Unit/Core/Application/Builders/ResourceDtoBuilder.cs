using Identity.Core.Application;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    internal class ResourceDtoBuilder
    {
        public static ResourceDto DefaultResourceDto => new ResourceDtoBuilder().Build();

        public string Id { get; private set; } = "MyResource";
        public string Description { get; private set; } = "My resource description.";

        public ResourceDtoBuilder WithId(string id)
        {
            this.Id = id;

            return this;
        }

        public ResourceDtoBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public ResourceDto Build()
            => new ResourceDto(
                this.Id,
                this.Description);
    }
}