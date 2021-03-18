using DDD.Application.Persistence.Adapters;
using Identity.Domain;
using System;

namespace Identity.Application
{
    using IAsyncRolesRepositoryAdapter = IAsyncRepositoryAdapter<RoleDto, Guid, IRolesRepository, RoleDtoConverter, Role, RoleId>;
    using IRolesRepositoryAdapter = IRepositoryAdapter<RoleDto, Guid, IRolesRepository, RoleDtoConverter, Role, RoleId>;

    internal class RolesRepositoryAdapter
    : IAsyncRolesRepositoryAdapter,
        IRolesRepositoryAdapter,
        Identity.Domain.IRolesRepository
    {
        public IRolesRepository RolesRepository { get; }

        IRolesRepository IAsyncRolesRepositoryAdapter.DtoRepository
            => this.RolesRepository;

        IRolesRepository IRolesRepositoryAdapter.DtoRepository
            => this.RolesRepository;

        public RolesRepositoryAdapter(IRolesRepository rolesRepository)
        {
            this.RolesRepository = rolesRepository
                ?? throw new ArgumentNullException(nameof(rolesRepository));
        }
    }
}