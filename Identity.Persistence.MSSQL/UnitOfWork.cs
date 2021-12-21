using DDD.Domain.Persistence;
using Identity.Core.Application;
using System;

namespace Identity.Persistence.MSSQL
{
    public class UnitOfWork : Identity.Core.Application.IUnitOfWork
    {
        public IApplicationsRepository ApplicationsRepository { get; }
        public IAuthorizationCodesRepository AuthorizationCodesRepository { get; }
        public IPermissionsRepository PermissionsRepository { get; }
        public IRefreshTokensRepository RefreshTokensRepository { get; }
        public IResourcesRepository ResourcesRepository { get; }
        public IRolesRepository RolesRepository { get; }
        public IUsersRepository UsersRepository { get; }

        public UnitOfWork(IdentityContext identityContext)
        {
            if(identityContext == null)
            {
                throw new ArgumentNullException(nameof(identityContext));
            }

            this.ApplicationsRepository = new ApplicationsRepository(identityContext);
            this.AuthorizationCodesRepository = new AuthorizationCodesRepository(identityContext);
            this.PermissionsRepository = new PermissionsRepository(identityContext);
            this.RefreshTokensRepository = new RefreshTokensRepository(identityContext);
            this.ResourcesRepository = new ResourcesRepository(identityContext);
            this.RolesRepository = new RolesRepository(identityContext);
            this.UsersRepository = new UsersRepository(identityContext);
        }

        public ITransactionScope BeginScope() 
            => new TransactionScope();
    }
}