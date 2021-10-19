using DDD.Application.Persistence.Adapters;
using System;

namespace Identity.Application
{
    using IAsyncAuthorizationCodesRepositoryAdapter = IAsyncRepositoryAdapter<AuthorizationCodeDto, (Guid ApplicationId, string Code), IAuthorizationCodesRepository, AuthorizationCodeDtoConverter, Domain.AuthorizationCode, Domain.AuthorizationCodeId>;
    using IAuthorizationCodesRepositoryAdapter = IRepositoryAdapter<AuthorizationCodeDto, (Guid ApplicationId, string Code), IAuthorizationCodesRepository, AuthorizationCodeDtoConverter, Domain.AuthorizationCode, Domain.AuthorizationCodeId>;

    internal class AuthorizationCodesRepositoryAdapter 
    : IAuthorizationCodesRepositoryAdapter, IAsyncAuthorizationCodesRepositoryAdapter, Domain.IAuthorizationCodesRepository
    {
        public IAuthorizationCodesRepository AuthorizationCodesRepository { get; }

        IAuthorizationCodesRepository IAsyncAuthorizationCodesRepositoryAdapter.DtoRepository
            => this.AuthorizationCodesRepository;

        IAuthorizationCodesRepository IAuthorizationCodesRepositoryAdapter.DtoRepository
            => this.AuthorizationCodesRepository;

        public AuthorizationCodesRepositoryAdapter(IAuthorizationCodesRepository authorizationCodesRepository)
        {
            this.AuthorizationCodesRepository = authorizationCodesRepository
                ?? throw new ArgumentNullException(nameof(authorizationCodesRepository));
        }
    }
}
