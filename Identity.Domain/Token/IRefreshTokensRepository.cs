using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IRefreshTokensRepository : IAsyncRepository<RefreshToken, TokenId>
    {
    }
}
