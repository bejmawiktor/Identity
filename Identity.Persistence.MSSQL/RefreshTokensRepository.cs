using DDD.Domain.Persistence;
using Identity.Core.Application;
using Identity.Persistence.MSSQL.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private IdentityContext Context { get; }

        public RefreshTokensRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(RefreshTokenDto refreshToken)
        {
            return this.Context.RefreshTokens
                .AddAsync(new RefreshToken(refreshToken))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<RefreshTokenDto> GetAsync(string id)
            => this.Context.RefreshTokens
                .FindAsync(new object[] { id })
                .AsTask()
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<RefreshTokenDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.RefreshTokens
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(r => r.Result.Select(r => r.ToDto()));
        }

        public Task RemoveAsync(RefreshTokenDto refreshToken)
        {
            return Task.Run(() => this.Remove(refreshToken))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(RefreshTokenDto refreshToken)
        {
            RefreshToken dataModel = this.Context
                .Find<RefreshToken>(new object[] { refreshToken.Id });

            this.Context.Remove(dataModel);
        }

        public Task UpdateAsync(RefreshTokenDto refreshToken)
        {
            return Task.Run(() => this.Update(refreshToken))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(RefreshTokenDto refreshToken)
        {
            RefreshToken dataModel = this.Context
                .Find<RefreshToken>(new object[] { refreshToken.Id });
            dataModel.SetFields(refreshToken);

            this.Context.Update(dataModel);
        }
    }
}