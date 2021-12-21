using DDD.Domain.Persistence;

namespace Identity.Core.Domain
{
    internal interface IAuthorizationCodesRepository : IAsyncRepository<AuthorizationCode, AuthorizationCodeId>
    {
    }
}