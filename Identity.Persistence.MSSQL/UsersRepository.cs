﻿using DDD.Domain.Persistence;
using Identity.Application;
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
                .FirstOrDefaultAsync(r => r.Id == id)
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
            return Task.Run(() => this.SetDeletedState(user))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void SetDeletedState(UserDto user)
        {
            var local = this.Context.Set<User>()
                .Local
                .FirstOrDefault(entry => entry.Id == user.Id);

            if (local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new User(user)).State = EntityState.Deleted;
        }

        public Task UpdateAsync(UserDto entity)
        {
            return Task.Run(() => this.SetModifiedState(entity))
                .ContinueWith((t) => _ = this.Context.SaveChangesAsync().Result);
        }

        private void SetModifiedState(UserDto user)
        {
            var local = this.Context.Set<User>()
                .Local
                .FirstOrDefault(entry => entry.Id == user.Id);

            if (local != null)
            {
                this.Context.Entry(local).State = EntityState.Detached;
            }

            this.Context.Entry(new User(user)).State = EntityState.Modified;
        }

        public Task<UserDto> GetAsync(string emailAddress)
            => this.Context.Users
                .FirstOrDefaultAsync(r => r.Email == emailAddress)
                .ContinueWith(r => r.Result?.ToDto());
    }
}