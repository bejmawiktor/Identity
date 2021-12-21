using DDD.Application.Persistence;

namespace Identity.Core.Application
{
    public interface IPermissionsRepository : IAsyncDtoRepository<PermissionDto, (string ResourceId, string Name)>
    {
    }
}