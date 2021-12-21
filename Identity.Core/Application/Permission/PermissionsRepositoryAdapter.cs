using DDD.Application.Persistence.Adapters;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    using IAsyncPermissionsRepositoryAdapter = IAsyncRepositoryAdapter<PermissionDto, (string ResourceId, string Name), IPermissionsRepository, PermissionDtoConverter, Permission, PermissionId>;

    internal class PermissionsRepositoryAdapter : IAsyncPermissionsRepositoryAdapter, Domain.IPermissionsRepository
    {
        public IPermissionsRepository PermissionsRepository { get; }

        IPermissionsRepository IAsyncPermissionsRepositoryAdapter.DtoRepository
            => this.PermissionsRepository;

        public PermissionsRepositoryAdapter(IPermissionsRepository permissionsRepository)
        {
            this.PermissionsRepository = permissionsRepository
                ?? throw new ArgumentNullException(nameof(permissionsRepository));
        }
    }
}