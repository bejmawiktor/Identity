using DDD.Application.Persistence.Adapters;
using Identity.Domain;
using System;

namespace Identity.Application
{
    using IAsyncPermissionsRepositoryAdapter = IAsyncRepositoryAdapter<PermissionDto, (string ResourceId, string Name), IPermissionsRepository, PermissionDtoConverter, Permission, PermissionId>;
    using IPermissionsRepositoryAdapter = IRepositoryAdapter<PermissionDto, (string ResourceId, string Name), IPermissionsRepository, PermissionDtoConverter, Permission, PermissionId>;

    internal class PermissionsRepositoryAdapter : IPermissionsRepositoryAdapter, IAsyncPermissionsRepositoryAdapter
    {
        public IPermissionsRepository PermissionsRepository { get; }

        IPermissionsRepository IAsyncPermissionsRepositoryAdapter.DtoRepository
            => this.PermissionsRepository;

        IPermissionsRepository IPermissionsRepositoryAdapter.DtoRepository
            => this.PermissionsRepository;

        public PermissionsRepositoryAdapter(IPermissionsRepository permissionsRepository)
        {
            this.PermissionsRepository = permissionsRepository
                ?? throw new ArgumentNullException(nameof(permissionsRepository));
        }
    }
}