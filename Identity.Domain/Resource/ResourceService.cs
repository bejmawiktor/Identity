using DDD.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class ResourceService
    {
        public IUnitOfWork UnitOfWork { get; }

        public ResourceService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork 
                ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task CreateResourceAsync(string name, string description)
        {
            using(EventsScope eventsScope = new EventsScope())
            {
                var resource = Resource.Create(name, description);

                await this.UnitOfWork.ResourcesRepository.AddAsync(resource);

                eventsScope.Publish();
            }
        }
    }
}