using DDD.Application.Persistence.Adapters;
using Identity.Domain;
using System;

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
    }
}