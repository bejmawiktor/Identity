using DDD.Application.Model;
using Identity.Domain;

namespace Identity.Application
{
    public record ResourceDto : IAggregateRootDto<Resource, ResourceId>
    {
        public string Id { get; }
        public string Description { get; }

        public ResourceDto(string id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        internal Resource ToResource()
            => new Resource(
                id: new ResourceId(this.Id),
                description: this.Description);

        Resource IDomainObjectDto<Resource>.ToDomainObject()
             => this.ToResource();
    }
}