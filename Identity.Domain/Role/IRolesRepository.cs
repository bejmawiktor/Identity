using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IRolesRepository
    : IRepository<Role, RoleId>, IAsyncRepository<Role, RoleId>
    {
    }
}