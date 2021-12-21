using DDD.Domain.Persistence;
using Identity.Core.Application;
using Identity.Persistence.MSSQL.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Persistence.MSSQL
{
    public class UsersRepository : IUsersRepository
    {
        private IdentityContext Context { get; }

        public UsersRepository(IdentityContext context)
        {
            this.Context = context;
        }

        public Task AddAsync(UserDto user)
        {
            return this.Context.Users
                .AddAsync(new User(user))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Task<UserDto> GetAsync(Guid id)
            => this.Context.Users
                .FindAsync(new object[] { id })
                .AsTask()
                .ContinueWith(r => r.Result?.ToDto());

        public Task<IEnumerable<UserDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Users
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToDto()));
        }

        public Task RemoveAsync(UserDto user)
        {
            return Task.Run(() => this.Remove(user))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Remove(UserDto user)
        {
            var dataModel = this.Context
                .Find<User>(new object[] { user.Id });

            this.Context.Remove(dataModel);
        }

        public Task UpdateAsync(UserDto user)
        {
            return Task.Run(() => this.Update(user))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void Update(UserDto user)
        {
            var dataModel = this.Context
                .Find<User>(new object[] { user.Id });
            dataModel.SetFields(user);

            this.Context.Update(dataModel);
        }

        public Task<UserDto> GetAsync(string emailAddress)
            => this.Context.Users
                .FirstOrDefaultAsync(r => r.Email == emailAddress)
                .ContinueWith(r => r.Result?.ToDto());
    }
}