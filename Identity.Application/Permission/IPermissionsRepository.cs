using DDD.Application.Persistence;

namespace Identity.Application
{
    public interface IPermissionsRepository
    : IAsyncDtoRepository<PermissionDto, (string ResourceId, string Name)>,
        IDtoRepository<PermissionDto, (string ResourceId, string Name)>
    {
    }
}