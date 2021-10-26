using DDD.Domain.Persistence;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public interface IUsersRepository : IAsyncRepository<User, UserId>
    {
        public Task<User> GetAsync(EmailAddress emailAddress);
    }
}