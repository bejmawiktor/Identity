using DDD.Application.Model.Converters;
using Identity.Domain;
using System;

namespace Identity.Application
{
    internal class ResourceDtoConverter : IAggregateRootDtoConverter<Resource, ResourceId, ResourceDto, string>
    {
        public ResourceDto ToDto(Resource resource)
        {
            if(resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            return new ResourceDto(resource.Id.Value, resource.Description);
        }

        public string ToDtoIdentifier(ResourceId resourceId)
        {
            if(resourceId == null)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            return resourceId.Value;
        }
    }
}