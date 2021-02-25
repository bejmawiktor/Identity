using DDD.Persistence;

namespace Identity.Domain
{
    public interface IResourcesRepository
    : IRepository<Resource, ResourceId>, IAsyncRepository<Resource, ResourceId>
    {
    }
}