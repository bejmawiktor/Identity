using DDD.Domain.Persistence;
using System.Threading.Tasks;

namespace Identity.Core.Domain
{
    internal interface IUsersRepository : IAsyncRepository<User, UserId>
    {
        public Task<User> GetAsync(EmailAddress emailAddress);
    }
}