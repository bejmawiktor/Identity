namespace Identity.Application
{
    public interface IUnitOfWork : DDD.Domain.Persistence.IUnitOfWork
    {
        IApplicationsRepository ApplicationsRepository { get; }
        IAuthorizationCodesRepository AuthorizationCodesRepository { get; }
        IUsersRepository UsersRepository { get; }
        IRefreshTokensRepository RefreshTokensRepository { get; }
        IPermissionsRepository PermissionsRepository { get; }
        IResourcesRepository ResourcesRepository { get; }
        IRolesRepository RolesRepository { get; }
    }
}
