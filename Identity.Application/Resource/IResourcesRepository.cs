using DDD.Application.Persistence;

namespace Identity.Application
{
    public interface IResourcesRepository : IAsyncDtoRepository<ResourceDto, string>
    {
    }
}