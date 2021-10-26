using DDD.Application.Persistence;
using System;

namespace Identity.Application
{
    public interface IRolesRepository : IAsyncDtoRepository<RoleDto, Guid>
    {
    }
}