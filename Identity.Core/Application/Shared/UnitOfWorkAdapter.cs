using DDD.Domain.Persistence;

namespace Identity.Core.Application
{
    internal class UnitOfWorkAdapter : Domain.IUnitOfWork
    {
        public Domain.IApplicationsRepository ApplicationsRepository { get; }
        public Domain.IAuthorizationCodesRepository AuthorizationCodesRepository { get; }
        public Domain.IPermissionsRepository PermissionsRepository { get; }
        public Domain.IResourcesRepository ResourcesRepository { get; }
        public Domain.IRolesRepository RolesRepository { get; }
        public Domain.IRefreshTokensRepository RefreshTokensRepository { get; }
        public Domain.IUsersRepository UsersRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public UnitOfWorkAdapter(IUnitOfWork unitOfWork)
        {
            this.ApplicationsRepository
                = new ApplicationsRepositoryAdapter(unitOfWork.ApplicationsRepository);
            this.AuthorizationCodesRepository
                = new AuthorizationCodesRepositoryAdapter(unitOfWork.AuthorizationCodesRepository);
            this.PermissionsRepository
                = new PermissionsRepositoryAdapter(unitOfWork.PermissionsRepository);
            this.ResourcesRepository
                = new ResourcesRepositoryAdapter(unitOfWork.ResourcesRepository);
            this.RolesRepository
                = new RolesRepositoryAdapter(unitOfWork.RolesRepository);
            this.RefreshTokensRepository
                = new RefreshTokensRepositoryAdapter(unitOfWork.RefreshTokensRepository);
            this.UsersRepository
                = new UsersRepositoryAdapter(unitOfWork.UsersRepository);
            this.UnitOfWork = unitOfWork;
        }

        public ITransactionScope BeginScope()
            => this.UnitOfWork.BeginScope();
    }
}