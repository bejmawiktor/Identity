using DDD.Application.Persistence;

namespace Identity.Application
{
    public interface IRefreshTokensRepository : IAsyncDtoRepository<RefreshTokenDto, string>
    {
    }
}
