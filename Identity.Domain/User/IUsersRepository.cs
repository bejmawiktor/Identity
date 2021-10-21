using DDD.Domain.Persistence;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public interface IUsersRepository
    : IRepository<User, UserId>, IAsyncRepository<User, UserId>
    {
        public User Get(EmailAddress emailAddress);

        public Task<User> GetAsync(EmailAddress emailAddress);
    }
}