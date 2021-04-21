using DDD.Application.Model;
using Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Application
{
    public class UserDto : IAggregateRootDto<User, UserId>
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

        public override bool Equals(object obj)
        {
            return obj is UserDto dto
                && this.Id.Equals(dto.Id)
                && this.Email == dto.Email
                && this.HashedPassword == dto.HashedPassword
                && this.Roles.SequenceEqual(dto.Roles)
                && this.Permissions.SequenceEqual(dto.Permissions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + this.Id.GetHashCode();
                hash = hash * 23 + this.Email?.GetHashCode() ?? 0;
                hash = hash * 23 + this.HashedPassword?.GetHashCode() ?? 0;

                foreach(var role in this.Roles)
                {
                    hash = hash * 23 + role.GetHashCode();
                }

                foreach(var permission in this.Permissions)
                {
                    hash = hash * 23 + permission.GetHashCode();
                }

                return hash;
            }
        }
    }
}