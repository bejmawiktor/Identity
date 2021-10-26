using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IResourcesRepository : IAsyncRepository<Resource, ResourceId>
    {
    }
}