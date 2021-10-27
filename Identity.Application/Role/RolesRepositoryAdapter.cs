using DDD.Application.Persistence.Adapters;
using Identity.Domain;
using System;

namespace Identity.Application
{
    using IAsyncRolesRepositoryAdapter = IAsyncRepositoryAdapter<RoleDto, Guid, IRolesRepository, RoleDtoConverter, Role, RoleId>;

    internal class RolesRepositoryAdapter : IAsyncRolesRepositoryAdapter, Domain.IRolesRepository
    {
        public IRolesRepository RolesRepository { get; }

        IRolesRepository IAsyncRolesRepositoryAdapter.DtoRepository
            => this.RolesRepository;

        public RolesRepositoryAdapter(IRolesRepository rolesRepository)
        {
            this.RolesRepository = rolesRepository
                ?? throw new ArgumentNullException(nameof(rolesRepository));
        }
    }
}