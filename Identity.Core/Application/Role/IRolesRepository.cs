using DDD.Application.Persistence;
using System;

namespace Identity.Core.Application
{
    public interface IRolesRepository : IAsyncDtoRepository<RoleDto, Guid>
    {
    }
}