using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Persistence.MSSQL.DataModels;
using Microsoft.EntityFrameworkCore;
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

        public void Add(PermissionDto permission)
        {
            this.Context.Permissions.Add(new Permission(permission));

            this.Context.SaveChanges();
        }

        public Task AddAsync(PermissionDto permission)
        {
            return this.Context.Permissions
                .AddAsync(new Permission(permission))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public PermissionDto Get((string ResourceId, string Name) id)
            => this.Context.Permissions
                .FirstOrDefault(r => r.Name == id.Name && r.ResourceId == id.ResourceId)?
                .ToDto();

        public IEnumerable<PermissionDto> Get(Pagination pagination)
        {
            return this.Context.Permissions
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage)
                .Select(r => r.ToDto());
        }

        public Task<PermissionDto> GetAsync((string ResourceId, string Name) id)
            => this.Context.Permissions
                .FirstOrDefaultAsync(r => r.Name == id.Name && r.ResourceId == id.ResourceId)
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<PermissionDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Permissions
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public void Remove(PermissionDto permission)
        {
            this.SetDeletedState(permission);

            this.Context.SaveChanges();
        }

        private void SetDeletedState(PermissionDto permission)
        {
            var local = this.Context.Set<Permission>()
                .Local
                .FirstOrDefault(entry => entry.Name == permission.Id.Name
                    && entry.ResourceId == permission.Id.ResourceId);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Permission(permission)).State = EntityState.Deleted;
        }

        public Task RemoveAsync(PermissionDto permission)
        {
            return Task.Run(() => this.SetDeletedState(permission))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public void Update(PermissionDto permission)
        {
            this.SetModifiedState(permission);

            this.Context.SaveChanges();
        }

        private void SetModifiedState(PermissionDto permission)
        {
            var local = this.Context.Set<Permission>()
                .Local
                .FirstOrDefault(entry => entry.Name == permission.Id.Name
                    && entry.ResourceId == permission.Id.ResourceId);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Permission(permission)).State = EntityState.Modified;
        }

        public Task UpdateAsync(PermissionDto permission)
        {
            return Task.Run(() => this.SetModifiedState(permission))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }
    }
}