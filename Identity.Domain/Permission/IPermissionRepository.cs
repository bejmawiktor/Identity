using DDD.Persistence;

namespace Identity.Domain
{
    public interface IPermissionRepository
    : IRepository<Permission, PermissionId>, IAsyncRepository<Permission, PermissionId>
    {
    }
}