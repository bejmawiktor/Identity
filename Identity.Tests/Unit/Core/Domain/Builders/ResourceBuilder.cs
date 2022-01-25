using Identity.Core.Domain;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class ResourceBuilder
    {
        public ResourceId Id { get; private set; } = new("TestResource");
        public string Description { get; private set; } = "Test resource description";

        public ResourceBuilder WithId(ResourceId id)
        {
            this.Id = id;

            return this;
        }

        public ResourceBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public Resource Build()
            => new Resource(
                this.Id,
                this.Description);
    }
}