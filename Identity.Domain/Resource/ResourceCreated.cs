using DDD.Domain.Events;

namespace Identity.Domain
{
    public class ResourceCreated : Event
    {
        public ResourceId ResourceId { get; }
        public string ResourceDescription { get; }

        internal ResourceCreated(ResourceId resourceId, string resourceDescription)
        {
            this.ResourceId = resourceId;
            this.ResourceDescription = resourceDescription;
        }
    }
}