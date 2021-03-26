using DDD.Domain.Persistence;
using Identity.Application;
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

        public void Add(Identity.Application.UserDto user)
        {
            this.Context.Users.Add(new UserDto(user));

            this.Context.SaveChanges();
        }

        public Task AddAsync(Identity.Application.UserDto user)
        {
            return this.Context.Users
                .AddAsync(new UserDto(user))
                .AsTask()
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public Identity.Application.UserDto Get(Guid id)
            => this.Context.Users
                .FirstOrDefault(r => r.Id == id)?
                .ToApplicationDto();

        public IEnumerable<Identity.Application.UserDto> Get(Pagination pagination)
        {
            return this.Context.Users
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage)
                .Select(r => r.ToApplicationDto());
        }

        public Task<Identity.Application.UserDto> GetAsync(Guid id)
            => this.Context.Users
                .FirstOrDefaultAsync(r => r.Id == id)
                .ContinueWith(r => r.Result?.ToApplicationDto());

        public Task<IEnumerable<Identity.Application.UserDto>> GetAsync(Pagination pagination = null)
        {
            return Task.Run(() => this.Context.Users
                .Skip((int)pagination.Page * (int)pagination.ItemsPerPage)
                .Take((int)pagination.ItemsPerPage).AsEnumerable())
                .ContinueWith(p => p.Result.Select(r => r.ToApplicationDto()));
        }

        public void Remove(Identity.Application.UserDto user)
        {
            this.SetDeletedState(user);

            this.Context.SaveChanges();
        }

        private void SetDeletedState(Identity.Application.UserDto user)
        {
            var local = this.Context.Set<UserDto>()
                .Local
                .FirstOrDefault(entry => entry.Id == user.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new UserDto(user)).State = EntityState.Deleted;
        }

        public Task RemoveAsync(Identity.Application.UserDto user)
        {
            return Task.Run(() => this.SetDeletedState(user))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        public void Update(Identity.Application.UserDto user)
        {
            this.SetModifiedState(user);

            this.Context.SaveChanges();
        }

        private void SetModifiedState(Identity.Application.UserDto user)
        {
            var local = this.Context.Set<UserDto>()
                .Local
                .FirstOrDefault(entry => entry.Id == user.Id);

            if(local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new UserDto(user)).State = EntityState.Modified;
        }

        public Task UpdateAsync(Identity.Application.UserDto entity)
        {
            return Task.Run(() => this.SetModifiedState(entity))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }
    }
}