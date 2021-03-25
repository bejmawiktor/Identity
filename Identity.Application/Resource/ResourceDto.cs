using DDD.Application.Model;
using Identity.Domain;
using System;

namespace Identity.Application
{
    public class ResourceDto : IAggregateRootDto<Resource, ResourceId>
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

        public override bool Equals(object obj)
        {
            return obj is ResourceDto dto
                && this.Id == dto.Id
                && this.Description == dto.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Description);
        }
    }
}