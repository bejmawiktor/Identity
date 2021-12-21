using DDD.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Core.Domain
{
    internal class PermissionService
    {
        public IUnitOfWork UnitOfWork { get; }

        public PermissionService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task CreatePermission(ResourceId resourceId, string name, string description)
        {
            using(var eventsScope = new EventsScope())
            {
                Resource resource = await this.UnitOfWork.ResourcesRepository.GetAsync(resourceId);

                if(resource == null)
                {
                    throw new ResourceNotFoundException(resourceId);
                }

                Permission permission = resource.CreatePermission(name, description);

                await this.UnitOfWork.PermissionsRepository.AddAsync(permission);

                eventsScope.Publish();
            }
        }
    }
}