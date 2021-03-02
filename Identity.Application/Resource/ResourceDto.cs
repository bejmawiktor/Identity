using DDD.Application.Model;
using Identity.Domain;

namespace Identity.Application
{
    public class ResourceDto : AggregateRootDto<Resource, ResourceId>
    {
        public string Id { get; }
        public string Description { get; }

        public ResourceDto(string id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        private ResourceDto()
        {
        }

        internal Resource ToResource()
            => this.ToDomainObject();

        protected override Resource ToDomainObject()
            => new Resource(
                id: new ResourceId(this.Id),
                description: this.Description);
    }
}