using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IApplicationsRepository : IAsyncRepository<Application, ApplicationId>
    {
    }
}