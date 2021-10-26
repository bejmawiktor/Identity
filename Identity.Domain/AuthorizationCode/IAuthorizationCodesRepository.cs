using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IAuthorizationCodesRepository : IAsyncRepository<AuthorizationCode, AuthorizationCodeId>
    {
    }
}
