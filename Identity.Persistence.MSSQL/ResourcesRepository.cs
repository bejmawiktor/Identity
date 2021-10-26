using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Persistence.MSSQL.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    public class ResourcesRepository : IResourcesRepository
    {
        private IdentityContext Context { get; }

        public ResourcesRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(ResourceDto resource)
        {
            return this.Context.Resources
                .AddAsync(new Resource(resource))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<ResourceDto> GetAsync(string id)
            => this.Context.Resources
                .FirstOrDefaultAsync(r => r.Id == id)
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<ResourceDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Resources
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(r => r.Result.Select(r => r.ToDto()));
        }

        public Task RemoveAsync(ResourceDto resource)
        {
            return Task.Run(() => this.SetDeletedState(resource))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void SetDeletedState(ResourceDto resource)
        {
            var local = this.Context.Set<Resource>()
               .Local
               .FirstOrDefault(entry => entry.Id == resource.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Resource(resource)).State = EntityState.Deleted;
        }

        public Task UpdateAsync(ResourceDto resource)
        {
            return Task.Run(() => this.SetModifiedState(resource))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void SetModifiedState(ResourceDto resource)
        {
            var local = this.Context.Set<Resource>()
                .Local
                .FirstOrDefault(entry => entry.Id == resource.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Resource(resource)).State = EntityState.Modified;
        }
    }
}