using DDD.Domain.Persistence;

namespace Identity.Core.Domain
{
    internal interface IResourcesRepository : IAsyncRepository<Resource, ResourceId>
    {
    }
}