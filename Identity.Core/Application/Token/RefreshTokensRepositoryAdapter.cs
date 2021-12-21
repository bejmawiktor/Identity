using DDD.Application.Persistence.Adapters;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    using IAsyncRefreshTokensRepositoryAdapter = IAsyncRepositoryAdapter<RefreshTokenDto, string, IRefreshTokensRepository, RefreshTokenDtoConverter, RefreshToken, TokenId>;

    public class RefreshTokensRepositoryAdapter : IAsyncRefreshTokensRepositoryAdapter, Domain.IRefreshTokensRepository
    {
        public IRefreshTokensRepository RefreshTokensRepository { get; }

        IRefreshTokensRepository IAsyncRefreshTokensRepositoryAdapter.DtoRepository
            => this.RefreshTokensRepository;

        public RefreshTokensRepositoryAdapter(IRefreshTokensRepository refreshTokensRepository)
        {
            this.RefreshTokensRepository = refreshTokensRepository 
                ?? throw new ArgumentNullException(nameof(refreshTokensRepository));
        }
    }
}
