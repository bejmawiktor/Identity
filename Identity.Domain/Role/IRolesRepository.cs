using DDD.Domain.Persistence;

namespace Identity.Domain
{
    public interface IRolesRepository : IAsyncRepository<Role, RoleId>
    {
    }
}