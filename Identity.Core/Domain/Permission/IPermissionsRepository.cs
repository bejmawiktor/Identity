using DDD.Domain.Persistence;

namespace Identity.Core.Domain
{
    internal interface IPermissionsRepository : IAsyncRepository<Permission, PermissionId>
    {
    }
}