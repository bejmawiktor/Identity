using DDD.Application.Persistence.Adapters;
using Identity.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.Core.Application
{
    using IAsyncUsersRepositoryAdapter = IAsyncRepositoryAdapter<UserDto, Guid, IUsersRepository, UserDtoConverter, User, UserId>;

    internal class UsersRepositoryAdapter : IAsyncUsersRepositoryAdapter, Domain.IUsersRepository
    {
        public IUsersRepository UsersRepository { get; }

        IUsersRepository IAsyncUsersRepositoryAdapter.DtoRepository
            => this.UsersRepository;

        public UsersRepositoryAdapter(IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository
                ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public Task<User> GetAsync(EmailAddress emailAddress)
            => this.UsersRepository
                .GetAsync(emailAddress.ToString())
                .ContinueWith(u => u.Result?.ToUser());
    }
}