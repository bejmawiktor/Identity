using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IAuthorizationCodesRepository
    : IRepository<AuthorizationCode, AuthorizationCodeId>, IAsyncRepository<AuthorizationCode, AuthorizationCodeId>
    {
    }
}
