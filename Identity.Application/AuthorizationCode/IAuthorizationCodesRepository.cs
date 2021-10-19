using DDD.Application.Persistence;
using System;

namespace Identity.Application
{
    public interface IAuthorizationCodesRepository : IAsyncDtoRepository<AuthorizationCodeDto, (Guid ApplicationId, string Code)>,
        IDtoRepository<AuthorizationCodeDto, (Guid ApplicationId, string Code)>
    {
    }
}
