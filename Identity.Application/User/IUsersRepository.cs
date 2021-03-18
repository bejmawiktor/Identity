using DDD.Application.Persistence;
using System;

namespace Identity.Application
{
    public interface IUsersRepository : IAsyncDtoRepository<UserDto, Guid>, IDtoRepository<UserDto, Guid>
    {
    }
}