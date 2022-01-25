using Identity.Core.Application;
using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    internal class UserDtoBuilder
    {
        public static readonly Guid DefaultId = Guid.NewGuid();
        public static readonly string DefaultPassword = HashedPassword.Hash(new Password("MyPassword")).ToString();
        public static readonly IEnumerable<Guid> DefaultRoles = new Guid[]
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        public static UserDto DefaultUserDto => new UserDtoBuilder().Build();

        public Guid Id { get; private set; } = UserDtoBuilder.DefaultId;
        public string Email { get; private set; } = "example@example.com";
        public string Password { get; private set; } = UserDtoBuilder.DefaultPassword;
        public IEnumerable<Guid> Roles { get; private set; } = UserDtoBuilder.DefaultRoles;
        public IEnumerable<(string ResourceId, string Name)> Permissions { get; private set; } = new (string ResourceId, string Name)[]
        {
            ("MyResource", "MyPermission"),
            ("MyResource2", "MyPermission2")
        };

        public UserDtoBuilder WithId(Guid id)
        {
            this.Id = id;

            return this;
        }

        public UserDtoBuilder WithEmail(string email)
        {
            this.Email = email;

            return this;
        }

        public UserDtoBuilder WithPassword(string password)
        {
            this.Password = password;

            return this;
        }

        public UserDtoBuilder WithRoles(IEnumerable<Guid> roles)
        {
            this.Roles = roles;

            return this;
        }

        public UserDtoBuilder WithPermissions(IEnumerable<(string ResourceId, string Name)> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public UserDto Build()
            => new UserDto(
                this.Id, 
                this.Email, 
                this.Password, 
                this.Roles, 
                this.Permissions);
    }
}