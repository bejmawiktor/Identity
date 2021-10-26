using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Persistence.MSSQL.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    public class RolesRepository : IRolesRepository
    {
        private IdentityContext Context { get; }

        public RolesRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(RoleDto role)
        {
            return this.Context.Roles
                .AddAsync(new Role(role))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<RoleDto> GetAsync(Guid id)
            => this.Context.Roles
                .FirstOrDefaultAsync(r => r.Id == id)
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<RoleDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Roles
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public Task RemoveAsync(RoleDto role)
        {
            return Task.Run(() => this.SetDeletedState(role))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void SetDeletedState(RoleDto role)
        {
            var local = this.Context.Set<Role>()
                .Local
                .FirstOrDefault(entry => entry.Id == role.Id);

            if (local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Role(role)).State = EntityState.Deleted;
        }

        public Task UpdateAsync(RoleDto role)
        {
            return Task.Run(() => this.SetModifiedState(role))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void SetModifiedState(RoleDto role)
        {
            var local = this.Context.Set<Role>()
                .Local
                .FirstOrDefault(entry => entry.Id == role.Id);

            if (local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Role(role)).State = EntityState.Modified;
        }
    }
}