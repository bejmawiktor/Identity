using DDD.Persistence;

namespace Identity.Domain
{
    public interface IPermissionsRepository
    : IRepository<Permission, PermissionId>, IAsyncRepository<Permission, PermissionId>
    {
    }
}