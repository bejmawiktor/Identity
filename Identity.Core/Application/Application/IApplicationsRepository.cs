using DDD.Application.Persistence;
using System;

namespace Identity.Core.Application
{
    public interface IApplicationsRepository : IAsyncDtoRepository<ApplicationDto, Guid>
    {
    }
}