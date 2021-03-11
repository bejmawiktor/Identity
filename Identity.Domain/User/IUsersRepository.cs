using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IUsersRepository
    : IRepository<User, UserId>, IAsyncRepository<User, UserId>
    {
    }
}