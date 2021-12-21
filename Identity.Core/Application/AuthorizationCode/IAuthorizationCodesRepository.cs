using DDD.Application.Persistence;
using System;

namespace Identity.Core.Application
{
    public interface IAuthorizationCodesRepository : IAsyncDtoRepository<AuthorizationCodeDto, (Guid ApplicationId, string Code)>
    {
    }
}