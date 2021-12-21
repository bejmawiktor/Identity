using DDD.Application.Persistence;

namespace Identity.Core.Application
{
    public interface IResourcesRepository : IAsyncDtoRepository<ResourceDto, string>
    {
    }
}