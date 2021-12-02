using DDD.Application.CQRS;
using Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    public class CreateResourceCommandHandler : IAsyncCommandHandler<CreateResourceCommand>
    {
        public IUsersRepository UsersRepository { get; }
        public IResourcesRepository ResourcesRepository { get; }
        public IRolesRepository RolesRepository { get; }
        public IApplicationsRepository ApplicationsRepository { get; }
        public IAuthorizationCodesRepository AuthorizationCodesRepository { get; }
        public IRefreshTokensRepository RefreshTokensRepository { get; }
        private ResourceService ResourceService { get; }
        private AuthorizationService AuthorizationService { get; }

        public CreateResourceCommandHandler(
            IResourcesRepository resourcesRepository,
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            IApplicationsRepository applicationsRepository,
            IAuthorizationCodesRepository authorizationCodesRepository,
            IRefreshTokensRepository refreshTokensRepository)
        {
            this.ResourcesRepository = resourcesRepository
                ?? throw new ArgumentNullException(nameof(resourcesRepository));
            this.UsersRepository = usersRepository
                ?? throw new ArgumentNullException(nameof(usersRepository));
            this.RolesRepository = rolesRepository
                ?? throw new ArgumentNullException(nameof(rolesRepository));
            this.ResourceService = new ResourceService(
                new ResourcesRepositoryAdapter(resourcesRepository));
            this.ApplicationsRepository = applicationsRepository
                ?? throw new ArgumentNullException(nameof(applicationsRepository));
            this.AuthorizationCodesRepository = authorizationCodesRepository
                ?? throw new ArgumentNullException(nameof(authorizationCodesRepository));
            this.RefreshTokensRepository = refreshTokensRepository 
                ?? throw new ArgumentNullException(nameof(refreshTokensRepository));
            this.AuthorizationService = new AuthorizationService(
                new UsersRepositoryAdapter(this.UsersRepository),
                new RolesRepositoryAdapter(this.RolesRepository),
                new ApplicationsRepositoryAdapter(this.ApplicationsRepository),
                new AuthorizationCodesRepositoryAdapter(this.AuthorizationCodesRepository),
                new RefreshTokensRepositoryAdapter(this.RefreshTokensRepository));
        }

        public async Task HandleAsync(CreateResourceCommand command)
        {
            await this.ValidateUserIsAuthorized(command);

            await this.ResourceService.CreateResourceAsync(command.ResourceId, command.ResourceDescription);
        }

        private async Task ValidateUserIsAuthorized(CreateResourceCommand command)
        {
            if(!await this.AuthorizationService.CheckUserAccess(new UserId(command.UserId), Permissions.CreateResource.Id))
            {
                throw new UnauthorizedAccessException("User isn't authorized to create resource.");
            }
        }
    }
}