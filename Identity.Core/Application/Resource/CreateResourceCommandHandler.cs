using Identity.Core.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Core.Application
{
    public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand>
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

        public async Task<Unit> Handle(CreateResourceCommand command, CancellationToken cancellationToken)
        {
            await this.ValidateUserIsAuthorized(command);

            await this.ResourceService.CreateResourceAsync(command.ResourceName, command.ResourceDescription);

            return await Unit.Task;
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