using DDD.Domain.Persistence;
using Identity.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    using Application = Identity.Persistence.MSSQL.DataModels.Application;

    public class ApplicationsRepository : IApplicationsRepository
    {
        private IdentityContext Context { get; }

        public ApplicationsRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(ApplicationDto application)
        {
            return this.Context.Applications
                .AddAsync(new Application(application))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<ApplicationDto> GetAsync(Guid id)
            => this.Context.Applications
                .FindAsync(new object[] { id })
                .AsTask()
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<ApplicationDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Applications
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public Task RemoveAsync(ApplicationDto application)
        {
            return Task.Run(() => this.Remove(application))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(ApplicationDto application)
        {
            Application dataModel = this.Context
                .Find<Application>(new object[] { application.Id });

            this.Context.Remove(dataModel);
        }

        public Task UpdateAsync(ApplicationDto application)
        {
            return Task.Run(() => this.Update(application))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(ApplicationDto application)
        {
            Application dataModel = this.Context
                .Find<Application>(new object[] { application.Id });
            dataModel.SetFields(application);

            this.Context.Update(dataModel);
        }
    }
}