using DDD.Application.Persistence.Adapters;
using Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.Application
{
    using IAsyncUsersRepositoryAdapter = IAsyncRepositoryAdapter<UserDto, Guid, IUsersRepository, UserDtoConverter, User, UserId>;
    using IUsersRepositoryAdapter = IRepositoryAdapter<UserDto, Guid, IUsersRepository, UserDtoConverter, User, UserId>;

    internal class UsersRepositoryAdapter
    : IAsyncUsersRepositoryAdapter, IUsersRepositoryAdapter, Domain.IUsersRepository
    {
        public IUsersRepository UsersRepository { get; }

        IUsersRepository IAsyncUsersRepositoryAdapter.DtoRepository
            => this.UsersRepository;

        IUsersRepository IUsersRepositoryAdapter.DtoRepository
            => this.UsersRepository;

        public UsersRepositoryAdapter(IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository
                ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public User Get(EmailAddress emailAddress)
            => this.UsersRepository
                .Get(emailAddress.ToString())?
                .ToUser();

        public Task<User> GetAsync(EmailAddress emailAddress)
            => this.UsersRepository
                .GetAsync(emailAddress.ToString())
                .ContinueWith(u => u.Result?.ToUser());
    }
}