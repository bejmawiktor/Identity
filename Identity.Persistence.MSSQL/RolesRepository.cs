using DDD.Domain.Persistence;
using Identity.Application;
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

        public void Add(Identity.Application.RoleDto role)
        {
            this.Context.Roles.Add(new RoleDto(role));

            this.Context.SaveChanges();
        }

        public Task AddAsync(Identity.Application.RoleDto role)
        {
            return this.Context.Roles
                .AddAsync(new RoleDto(role))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Identity.Application.RoleDto Get(Guid id)
            => this.Context.Roles
                .FirstOrDefault(r => r.Id == id)?
                .ToApplicationDto();

        public IEnumerable<Identity.Application.RoleDto> Get(Pagination pagination)
        {
            return this.Context.Roles
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage)
                .Select(r => r.ToApplicationDto());
        }

        public Task<Identity.Application.RoleDto> GetAsync(Guid id)
            => this.Context.Roles
                .FirstOrDefaultAsync(r => r.Id == id)
                .ContinueWith(r => r.Result?.ToApplicationDto());

        public Task<IEnumerable<Identity.Application.RoleDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Roles
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToApplicationDto()));
        }

        public void Remove(Identity.Application.RoleDto role)
        {
            this.SetDeletedState(role);

            this.Context.SaveChanges();
        }

        private void SetDeletedState(Identity.Application.RoleDto role)
        {
            var local = this.Context.Set<RoleDto>()
                .Local
                .FirstOrDefault(entry => entry.Id == role.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new RoleDto(role)).State = EntityState.Deleted;
        }

        public Task RemoveAsync(Identity.Application.RoleDto role)
        {
            return Task.Run(() => this.SetDeletedState(role))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public void Update(Identity.Application.RoleDto role)
        {
            this.SetModifiedState(role);

            this.Context.SaveChanges();
        }

        private void SetModifiedState(Identity.Application.RoleDto role)
        {
            var local = this.Context.Set<RoleDto>()
                .Local
                .FirstOrDefault(entry => entry.Id == role.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new RoleDto(role)).State = EntityState.Modified;
        }

        public Task UpdateAsync(Identity.Application.RoleDto entity)
        {
            return Task.Run(() => this.SetModifiedState(entity))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }
    }
}