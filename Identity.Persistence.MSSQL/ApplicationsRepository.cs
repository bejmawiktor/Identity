using DDD.Domain.Persistence;
using Identity.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    using Application = Identity.Persistence.MSSQL.DataModels.Application;

    public class ApplicationsRepository
    {
        private IdentityContext Context { get; }

        public ApplicationsRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public void Add(ApplicationDto application)
        {
            this.Context.Applications.Add(new Application(application));

            this.Context.SaveChanges();
        }

        public Task AddAsync(ApplicationDto application)
        {
            return this.Context.Applications
                .AddAsync(new Application(application))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public ApplicationDto Get(Guid id)
            => this.Context.Applications
                .FirstOrDefault(r => r.Id == id)?
                .ToDto();

        public IEnumerable<ApplicationDto> Get(Pagination pagination)
        {
            return this.Context.Applications
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage)
                .Select(r => r.ToDto());
        }

        public Task<ApplicationDto> GetAsync(Guid id)
            => this.Context.Applications
                .FirstOrDefaultAsync(r => r.Id == id)
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<ApplicationDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Applications
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public void Remove(ApplicationDto application)
        {
            this.SetDeletedState(application);

            this.Context.SaveChanges();
        }

        private void SetDeletedState(ApplicationDto application)
        {
            var local = this.Context.Set<Application>()
                .Local
                .FirstOrDefault(entry => entry.Id == application.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Application(application)).State = EntityState.Deleted;
        }

        public Task RemoveAsync(ApplicationDto application)
        {
            return Task.Run(() => this.SetDeletedState(application))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public void Update(ApplicationDto application)
        {
            this.SetModifiedState(application);

            this.Context.SaveChanges();
        }

        private void SetModifiedState(ApplicationDto application)
        {
            var local = this.Context.Set<Application>()
                .Local
                .FirstOrDefault(entry => entry.Id == application.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new Application(application)).State = EntityState.Modified;
        }

        public Task UpdateAsync(ApplicationDto application)
        {
            return Task.Run(() => this.SetModifiedState(application))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }
    }
}