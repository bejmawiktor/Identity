using DDD.Domain.Persistence;

namespace Identity.Core.Domain
{
    internal interface IRefreshTokensRepository : IAsyncRepository<RefreshToken, TokenId>
    {
    }
}