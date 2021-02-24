using DDD.Persistence;

namespace Identity.Domain
{
    public interface IResourceRepository
    : IRepository<Resource, ResourceId>, IAsyncRepository<Resource, ResourceId>
    {
    }
}