using DDD.Application.CQRS;
using Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    public class CreateResourceCommandHandler
    : ICommandHandler<CreateResourceCommand>, IAsyncCommandHandler<CreateResourceCommand>
    {
        public IUsersRepository UsersRepository { get; }
        public IResourcesRepository ResourcesRepository { get; }
        public IRolesRepository RolesRepository { get; }
        private ResourceService ResourceService { get; }
        private AuthorizationService AuthorizationService { get; }

        public CreateResourceCommandHandler(
            IResourcesRepository resourcesRepository,
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository)
        {
            this.ResourcesRepository = resourcesRepository
                ?? throw new ArgumentNullException(nameof(resourcesRepository));
            this.UsersRepository = usersRepository
                ?? throw new ArgumentNullException(nameof(usersRepository));
            this.RolesRepository = rolesRepository
                ?? throw new ArgumentNullException(nameof(rolesRepository));
            this.ResourceService = new ResourceService(
                new ResourcesRepositoryAdapter(resourcesRepository));
            this.AuthorizationService = new AuthorizationService(
                new UsersRepositoryAdapter(this.UsersRepository),
                new RolesRepositoryAdapter(this.RolesRepository));
        }

        public void Handle(CreateResourceCommand command)
        {
            this.ValidateUserIsAuthorized(command);

            this.ResourceService.CreateResource(command.ResourceId, command.ResourceDescription);
        }

        private void ValidateUserIsAuthorized(CreateResourceCommand command)
        {
            if(!this.AuthorizationService.CheckUserAccess(new UserId(command.UserId), Permissions.CreateResource.Id))
            {
                throw new UnauthorizedAccessException("User isn't authorized to create resource.");
            }
        }

        public Task HandleAsync(CreateResourceCommand command)
        {
            this.ValidateUserIsAuthorized(command);

            return this.ResourceService.CreateResourceAsync(command.ResourceId, command.ResourceDescription);
        }
    }
}