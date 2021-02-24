using DDD.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class ResourceService
    {
        public IResourceRepository ResourceRepository { get; }

        public ResourceService(IResourceRepository resourceRepository)
        {
            this.ResourceRepository = resourceRepository 
                ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public void CreateResource(string name, string description)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                var resource = Resource.Create(name, description);

                this.ResourceRepository.Add(resource);

                eventsScope.Publish();
            }
        }

        public async Task CreateResourceAsync(string name, string description)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                var resource = Resource.Create(name, description);

                await this.ResourceRepository.AddAsync(resource);

                eventsScope.Publish();
            }
        }
    }
}