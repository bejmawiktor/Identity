using DDD.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class PermissionService
    {
        public IPermissionsRepository PermissionsRepository { get; }
        public IResourcesRepository ResourcesRepository { get; }

        public PermissionService(
            IPermissionsRepository permissionsRepository,
            IResourcesRepository resourcesRepository)
        {
            this.PermissionsRepository = permissionsRepository
                ?? throw new ArgumentNullException(nameof(permissionsRepository));
            this.ResourcesRepository = resourcesRepository
                ?? throw new ArgumentNullException(nameof(resourcesRepository));
        }

        public void CreatePermission(ResourceId resourceId, string name, string description)
        {
            using(var eventsScope = new EventsScope())
            {
                Resource resource = this.ResourcesRepository.Get(resourceId);

                if(resource == null)
                {
                    throw new ResourceNotFoundException(resourceId);
                }

                Permission permission = resource.CreatePermission(name, description);

                this.PermissionsRepository.Add(permission);

                eventsScope.Publish();
            }
        }

        public async Task CreatePermissionAsync(ResourceId resourceId, string name, string description)
        {
            using (var eventsScope = new EventsScope())
            {
                Resource resource = await this.ResourcesRepository.GetAsync(resourceId);

                if(resource == null)
                {
                    throw new ResourceNotFoundException(resourceId);
                }

                Permission permission = resource.CreatePermission(name, description);

                await this.PermissionsRepository.AddAsync(permission);

                eventsScope.Publish();
            }
        }
    }
}