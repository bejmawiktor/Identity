using DDD.Domain.Persistence;
using Identity.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Persistence.MSSQL.DataModels;

namespace Identity.Persistence.MSSQL
{
    public class RolesRepository : IRolesRepository
    {
        private IdentityContext Context { get; }

        public RolesRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public void Add(RoleDto role)
        {
            this.Context.Roles.Add(new Role(role));

            this.Context.SaveChanges();
        }

        public Task AddAsync(RoleDto role)
        {
            return this.Context.Roles
                .AddAsync(new Role(role))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public RoleDto Get(Guid id)
            => this.Context.Roles
                .FirstOrDefault(r => r.Id == id)?
                .ToDto();

        public IEnumerable<RoleDto> Get(Pagination pagination)
        {
            return this.Context.Roles
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage)
                .Select(r => r.ToDto());
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

        public void Remove(RoleDto role)
        {
            this.SetDeletedState(role);

            this.Context.SaveChanges();
        }

        private void SetDeletedState(RoleDto role)
        {
            var local = this.Context.Set<Role>()
                .Local
                .FirstOrDefault(entry => entry.Id == role.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Role(role)).State = EntityState.Deleted;
        }

        public Task RemoveAsync(RoleDto role)
        {
            return Task.Run(() => this.SetDeletedState(role))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public void Update(RoleDto role)
        {
            this.SetModifiedState(role);

            this.Context.SaveChanges();
        }

        private void SetModifiedState(RoleDto role)
        {
            var local = this.Context.Set<Role>()
                .Local
                .FirstOrDefault(entry => entry.Id == role.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Role(role)).State = EntityState.Modified;
        }

        public Task UpdateAsync(RoleDto entity)
        {
            return Task.Run(() => this.SetModifiedState(entity))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }
    }
}