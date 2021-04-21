using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IApplicationsRepository
    : IRepository<Application, ApplicationId>, IAsyncRepository<Application, ApplicationId>
    {
    }
}