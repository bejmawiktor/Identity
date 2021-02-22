using DDD.Events;

namespace Identity.Domain
{
    public class ResourceCreatedEvent : Event
    {
        public ResourceId ResourceId { get; }
        public string Description { get; }

        internal ResourceCreatedEvent(ResourceId resourceId, string description)
        {
            this.ResourceId = resourceId;
            this.Description = description;
        }
    }
}