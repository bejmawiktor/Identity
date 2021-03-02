using DDD.Application.Persistence;

namespace Identity.Application
{
    public interface IResourcesRepository : IAsyncDtoRepository<ResourceDto, string>, IDtoRepository<ResourceDto, string>
    {
    }
}