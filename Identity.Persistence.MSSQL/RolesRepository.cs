using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Persistence.MSSQL.DataModels;
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
                .FindAsync(new object[] { id })
                .AsTask()
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
            return Task.Run(() => this.Remove(role))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(RoleDto role)
        {
            var dataModel = this.Context
                .Find<Role>(new object[] { role.Id });

            this.Context.Remove(dataModel);
        }

        public Task UpdateAsync(RoleDto role)
        {
            return Task.Run(() => this.Update(role))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(RoleDto role)
        {
            var dataModel = this.Context
                .Find<Role>(new object[] { role.Id });
            dataModel.SetFields(role);

            this.Context.Update(dataModel);
        }
    }
}