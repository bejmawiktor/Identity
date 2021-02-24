using DDD.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class PermissionService
    {
        public IPermissionRepository PermissionRepository { get; }
        public IResourceRepository ResourceRepository { get; }

        public PermissionService(
            IPermissionRepository permissionRepository,
            IResourceRepository resourceRepository)
        {
            this.PermissionRepository = permissionRepository
                ?? throw new ArgumentNullException(nameof(permissionRepository));
            this.ResourceRepository = resourceRepository
                ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public void CreatePermission(ResourceId resourceId, string name, string description)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                Resource resource = this.ResourceRepository.Get(resourceId);

                if(resource == null)
                {
                    throw new ResourceNotFoundException(resourceId);
                }

                Permission permission = resource.CreatePermission(name, description);

                this.PermissionRepository.Add(permission);

                eventsScope.Publish();
            }
        }

        public async Task CreatePermissionAsync(ResourceId resourceId, string name, string description)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                Resource resource = await this.ResourceRepository.GetAsync(resourceId);

                if(resource == null)
                {
                    throw new ResourceNotFoundException(resourceId);
                }

                Permission permission = resource.CreatePermission(name, description);

                await this.PermissionRepository.AddAsync(permission);

                eventsScope.Publish();
            }
        }
    }
}