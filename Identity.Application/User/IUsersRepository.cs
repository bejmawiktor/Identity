using DDD.Application.Persistence;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    public interface IUsersRepository : IAsyncDtoRepository<UserDto, Guid>, IDtoRepository<UserDto, Guid>
    {
        public UserDto Get(string emailAddress);

        public Task<UserDto> GetAsync(string emailAddress);
    }
}