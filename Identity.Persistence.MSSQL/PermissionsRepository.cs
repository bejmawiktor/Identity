using DDD.Domain.Persistence;
using Identity.Core.Application;
using Identity.Persistence.MSSQL.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private IdentityContext Context { get; }

        public PermissionsRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(PermissionDto permission)
        {
            return this.Context.Permissions
                .AddAsync(new Permission(permission))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<PermissionDto> GetAsync((string ResourceId, string Name) id)
            => this.Context.Permissions
                .FindAsync(new object[] { id.Name, id.ResourceId })
                .AsTask()
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<PermissionDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Permissions
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public Task RemoveAsync(PermissionDto permission)
        {
            return Task.Run(() => this.Remove(permission))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(PermissionDto permission)
        {
            Permission dataModel = this.Context
                .Find<Permission>(new object[] { permission.Id.Name, permission.Id.ResourceId });

            this.Context.Remove(dataModel);
        }

        public Task UpdateAsync(PermissionDto permission)
        {
            return Task.Run(() => this.Update(permission))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(PermissionDto permission)
        {
            Permission dataModel = this.Context
                .Find<Permission>(new object[] { permission.Id.Name, permission.Id.ResourceId });
            dataModel.SetFields(permission);

            this.Context.Update(dataModel);
        }
    }
}