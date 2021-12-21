using DDD.Application.Persistence;
using System;
using System.Threading.Tasks;

namespace Identity.Core.Application
{
    public interface IUsersRepository : IAsyncDtoRepository<UserDto, Guid>
    {
        public Task<UserDto> GetAsync(string emailAddress);
    }
}