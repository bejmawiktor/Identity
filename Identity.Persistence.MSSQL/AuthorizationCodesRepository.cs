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
    using AuthorizationCode = Identity.Persistence.MSSQL.DataModels.AuthorizationCode;

    public class AuthorizationCodesRepository
    {
        private IdentityContext Context { get; }

        public AuthorizationCodesRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public void Add(AuthorizationCodeDto authorizationCodeDto)
        {
            this.Context.AuthorizationCodes.Add(new AuthorizationCode(authorizationCodeDto));

            this.Context.SaveChanges();
        }

        public Task AddAsync(AuthorizationCodeDto authorizationCodeDto)
        {
            return this.Context.AuthorizationCodes
                .AddAsync(new AuthorizationCode(authorizationCodeDto))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public AuthorizationCodeDto Get((Guid ApplicationId, string Code) id)
            => this.Context.AuthorizationCodes
                .FirstOrDefault(r => r.Code == id.Code && r.ApplicationId == id.ApplicationId)?
                .ToDto();

        public Task<AuthorizationCodeDto> GetAsync((Guid ApplicationId, string Code) id)
            => this.Context.AuthorizationCodes
                .FirstOrDefaultAsync(r => r.Code == id.Code && r.ApplicationId == id.ApplicationId)
                .ContinueWith(r => r.Result?.ToDto());

        public IEnumerable<AuthorizationCodeDto> Get(Pagination pagination)
        {
            return this.Context.AuthorizationCodes
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage)
                .Select(r => r.ToDto());
        }

        public Task<IEnumerable<AuthorizationCodeDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.AuthorizationCodes
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public void Update(AuthorizationCodeDto authorizationCode)
        {
            this.SetModifiedState(authorizationCode);

            this.Context.SaveChanges();
        }

        private void SetModifiedState(AuthorizationCodeDto authorizationCode)
        {
            var local = this.Context.Set<AuthorizationCode>()
                .Local
                .FirstOrDefault(entry => entry.Code == authorizationCode.Code 
                    && entry.ApplicationId == authorizationCode.ApplicationId);

            if (local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new AuthorizationCode(authorizationCode)).State = EntityState.Modified;
        }

        public Task UpdateAsync(AuthorizationCodeDto authorizationCode)
        {
            return Task.Run(() => this.SetModifiedState(authorizationCode))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public void Remove(AuthorizationCodeDto authorizationCode)
        {
            this.SetDeletedState(authorizationCode);

            this.Context.SaveChanges();
        }

        private void SetDeletedState(AuthorizationCodeDto authorizationCode)
        {
            var local = this.Context.Set<AuthorizationCode>()
                .Local
                .FirstOrDefault(entry => entry.Code == authorizationCode.Code
                    && entry.ApplicationId == authorizationCode.ApplicationId);

            if (local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new AuthorizationCode(authorizationCode)).State = EntityState.Deleted;
        }

        public Task RemoveAsync(AuthorizationCodeDto authorizationCode)
        {
            return Task.Run(() => this.SetDeletedState(authorizationCode))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }
    }
}
