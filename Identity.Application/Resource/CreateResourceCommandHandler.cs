using DDD.Application.CQRS;
using Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    public class CreateResourceCommandHandler
    : ICommandHandler<CreateResourceCommand>, IAsyncCommandHandler<CreateResourceCommand>
    {
        public IResourcesRepository ResourcesRepository { get; }
        private ResourceService ResourceService { get; }

        public CreateResourceCommandHandler(IResourcesRepository resourcesRepository)
        {
            this.ResourcesRepository = resourcesRepository
                ?? throw new ArgumentNullException(nameof(resourcesRepository));
            this.ResourceService = new ResourceService(
                new ResourcesRepositoryAdapter(resourcesRepository));
        }

        public void Handle(CreateResourceCommand command)
        {
            this.ResourceService.CreateResource(command.ResourceId, command.ResourceDescription);
        }

        public Task HandleAsync(CreateResourceCommand command)
        {
            return this.ResourceService.CreateResourceAsync(command.ResourceId, command.ResourceDescription);
        }
    }
}