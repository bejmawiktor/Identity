using DDD.Events;

namespace Identity.Domain
{
    public class ResourceCreatedEvent : Event
    {
        public ResourceId ResourceId { get; }
        public string ResourceDescription { get; }

        internal ResourceCreatedEvent(ResourceId resourceId, string resourceDescription)
        {
            this.ResourceId = resourceId;
            this.ResourceDescription = resourceDescription;
        }
    }
}