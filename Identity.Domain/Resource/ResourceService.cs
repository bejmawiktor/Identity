using DDD.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class ResourceService
    {
        public IResourcesRepository ResourcesRepository { get; }

        public ResourceService(IResourcesRepository resourcesRepository)
        {
            this.ResourcesRepository = resourcesRepository
                ?? throw new ArgumentNullException(nameof(resourcesRepository));
        }

        public void CreateResource(string name, string description)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                var resource = Resource.Create(name, description);

                this.ResourcesRepository.Add(resource);

                eventsScope.Publish();
            }
        }

        public async Task CreateResourceAsync(string name, string description)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                var resource = Resource.Create(name, description);

                await this.ResourcesRepository.AddAsync(resource);

                eventsScope.Publish();
            }
        }
    }
}