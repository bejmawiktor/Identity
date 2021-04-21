using DDD.Application.Persistence;
using System;

namespace Identity.Application
{
    public interface IApplicationsRepository
    : IAsyncDtoRepository<ApplicationDto, Guid>, IDtoRepository<ApplicationDto, Guid>
    {
    }
}