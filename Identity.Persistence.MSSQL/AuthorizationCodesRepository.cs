using DDD.Domain.Persistence;
using Identity.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    using AuthorizationCode = Identity.Persistence.MSSQL.DataModels.AuthorizationCode;

    public class AuthorizationCodesRepository : IAuthorizationCodesRepository
    {
        private IdentityContext Context { get; }

        public AuthorizationCodesRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(AuthorizationCodeDto authorizationCodeDto)
        {
            return this.Context.AuthorizationCodes
                .AddAsync(new AuthorizationCode(authorizationCodeDto))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<AuthorizationCodeDto> GetAsync((Guid ApplicationId, string Code) id)
            => this.Context.AuthorizationCodes
                .FindAsync(new object[] { id.Code, id.ApplicationId })
                .AsTask()
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<AuthorizationCodeDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.AuthorizationCodes
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public Task UpdateAsync(AuthorizationCodeDto authorizationCode)
        {
            return Task.Run(() => this.Update(authorizationCode))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(AuthorizationCodeDto authorizationCode)
        {
            var dataModel = this.Context
                .Find<AuthorizationCode>(new object[] { authorizationCode.Code, authorizationCode.ApplicationId });
            dataModel.SetFields(authorizationCode);

            this.Context.Update(dataModel);
        }

        public Task RemoveAsync(AuthorizationCodeDto authorizationCode)
        {
            return Task.Run(() => this.Remove(authorizationCode))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(AuthorizationCodeDto authorizationCode)
        {
            var dataModel = this.Context
                .Find<AuthorizationCode>(new object[] { authorizationCode.Code, authorizationCode.ApplicationId });

            this.Context.Remove(dataModel);
        }
    }
}