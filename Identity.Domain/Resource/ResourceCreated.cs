using DDD.Domain.Events;

namespace Identity.Domain
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