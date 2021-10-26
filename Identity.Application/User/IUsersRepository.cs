using DDD.Application.Persistence;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    public interface IUsersRepository : IAsyncDtoRepository<UserDto, Guid>
    {
        public Task<UserDto> GetAsync(string emailAddress);
    }
}