using DDD.Domain.Persistence;
using Identity.Core.Application;
using Identity.Persistence.MSSQL.DataModels;
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
                .FindAsync(new object[] { id })
                .AsTask()
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
            return Task.Run(() => this.Remove(resource))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(ResourceDto resource)
        {
            Resource dataModel = this.Context
                .Find<Resource>(new object[] { resource.Id });

            this.Context.Remove(dataModel);
        }

        public Task UpdateAsync(ResourceDto resource)
        {
            return Task.Run(() => this.Update(resource))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(ResourceDto resource)
        {
            Resource dataModel = this.Context
                .Find<Resource>(new object[] { resource.Id });
            dataModel.SetFields(resource);

            this.Context.Update(dataModel);
        }
    }
}