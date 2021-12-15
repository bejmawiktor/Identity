using DDD.Application.CQRS;
using Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    public class CreateResourceCommandHandler : IAsyncCommandHandler<CreateResourceCommand>
    {
        private ResourceService ResourceService { get; }
        private AuthorizationService AuthorizationService { get; }
        private UnitOfWorkAdapter UnitOfWorkAdapter { get; }

        public CreateResourceCommandHandler(IUnitOfWork unitOfWork)
        {
            this.UnitOfWorkAdapter = new UnitOfWorkAdapter(
                unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)));
            this.ResourceService = new ResourceService(this.UnitOfWorkAdapter);
            this.AuthorizationService = new AuthorizationService(this.UnitOfWorkAdapter);
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