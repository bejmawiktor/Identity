using DDD.Application.Persistence;

namespace Identity.Core.Application
{
    public interface IRefreshTokensRepository : IAsyncDtoRepository<RefreshTokenDto, string>
    {
    }
}
