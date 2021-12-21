using DDD.Domain.Events;
using Identity.Core.Domain;

namespace Identity.Core.Events
{
    public class ResourceCreated : Event
    {
        public string ResourceId { get; }
        public string ResourceDescription { get; }

        internal ResourceCreated(ResourceId resourceId, string resourceDescription)
        {
            this.ResourceId = resourceId.ToString();
            this.ResourceDescription = resourceDescription;
        }
    }
}