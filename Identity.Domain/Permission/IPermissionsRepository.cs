using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IPermissionsRepository : IAsyncRepository<Permission, PermissionId>
    {
    }
}