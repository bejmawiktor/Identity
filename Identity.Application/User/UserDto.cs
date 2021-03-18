using DDD.Application.Model;
using Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Application
{
    public record UserDto : IAggregateRootDto<User, UserId>
    {
        public Guid Id { get; }
        public string Email { get; }
        public string HashedPassword { get; }
        public IEnumerable<Guid> Roles { get; }
        public IEnumerable<(string ResourceId, string Name)> Permissions { get; }

        public UserDto(
            Guid id,
            string email,
            string hashedPassword,
            IEnumerable<Guid> roles = null,
            IEnumerable<(string ResourceId, string Name)> permissions = null)
        {
            this.Id = id;
            this.Email = email;
            this.HashedPassword = hashedPassword;
            this.Roles = roles ?? Enumerable.Empty<Guid>();
            this.Permissions = permissions ?? Enumerable.Empty<(string ResourceId, string Name)>();
        }

        internal User ToUser()
            => new User(
                id: new UserId(this.Id),
                email: new EmailAddress(this.Email),
                password: new HashedPassword(this.HashedPassword),
                roles: this.ConvertRoles(),
                permissions: this.ConvertPermissions());

        private IEnumerable<RoleId> ConvertRoles()
            => this.Roles.Select(r => new RoleId(r));

        private IEnumerable<PermissionId> ConvertPermissions()
            => this.Permissions.Select(p => this.CreatePermissionId(p));

        private PermissionId CreatePermissionId((string ResourceId, string Name) permissionIdTuple)
            => new PermissionId(new ResourceId(permissionIdTuple.ResourceId), permissionIdTuple.Name);

        User IDomainObjectDto<User>.ToDomainObject()
            => this.ToUser();
    }
}