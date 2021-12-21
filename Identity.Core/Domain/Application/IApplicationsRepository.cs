using DDD.Domain.Persistence;

namespace Identity.Core.Domain
{
    internal interface IApplicationsRepository : IAsyncRepository<Application, ApplicationId>
    {
    }
}