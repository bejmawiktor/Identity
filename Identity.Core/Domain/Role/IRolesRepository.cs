using DDD.Domain.Persistence;

namespace Identity.Core.Domain
{
    internal interface IRolesRepository : IAsyncRepository<Role, RoleId>
    {
    }
}