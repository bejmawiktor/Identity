using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class ResourceCreatedBuilder
    {
        public ResourceId ResourceId { get; private set; } = new("TestResource");
        public string ResourceDescription { get; private set; } = "Test resource description";

        public ResourceCreatedBuilder WithResourceId(ResourceId resourceId)
        {
            this.ResourceId = resourceId;

            return this;
        }

        public ResourceCreatedBuilder WithResourceDescription(string resourceDescription)
        {
            this.ResourceDescription = resourceDescription;

            return this;
        }

        public ResourceCreated Build()
            => new ResourceCreated(
                this.ResourceId,
                this.ResourceDescription);
    }
}