using DDD.Domain.Events;
using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain
{
    public class User : PermissionHolder<UserId>, IAggregateRoot<UserId>
    {
        private EmailAddress email;
        private HashedPassword password;
        protected List<RoleId> roles;

        public EmailAddress Email
        {
            get => this.email;
            set
            {
                this.ValidateEmail(value);

                this.email = value;
            }
        }

        private void ValidateEmail(EmailAddress email)
        {
            if(email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
        }

        public HashedPassword Password
        {
            get => this.password;
            set
            {
                this.ValidatePassword(value);

                this.password = value;
            }
        }

        private void ValidatePassword(HashedPassword password)
        {
            if(password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
        }

        public IReadOnlyCollection<RoleId> Roles
            => this.roles.AsReadOnly();

        public User(
            UserId id,
            EmailAddress email,
            HashedPassword password,
            IEnumerable<RoleId> roles = null,
            IEnumerable<PermissionId> permissions = null)
        : base(id, permissions)
        {
            this.Email = email;
            this.Password = password;
            this.roles = roles?.ToList() ?? new List<RoleId>();
        }

        public static User Create(EmailAddress email, HashedPassword password)
        {
            var user = new User(UserId.Generate(), email, password);

            EventManager.Instance.Notify(new UserCreated(
                user.Id,
                user.Email));

            return user;
        }

        public override void ObtainPermission(PermissionId permissionId)
        {
            base.ObtainPermission(permissionId);

            EventManager.Instance.Notify(new UserPermissionObtained(this.Id, permissionId));
        }

        public override void RevokePermission(PermissionId permissionId)
        {
            base.RevokePermission(permissionId);

            EventManager.Instance.Notify(new UserPermissionRevoked(this.Id, permissionId));
        }

        public bool HasRole(RoleId roleId)
        {
            if(roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            return this.Roles.Contains(roleId);
        }

        public void AssumeRole(RoleId roleId)
        {
            if(roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            if(this.HasRole(roleId))
            {
                throw new InvalidOperationException("Role was already assumed.");
            }

            this.roles.Add(roleId);

            EventManager.Instance.Notify(new UserRoleAssumed(this.Id, roleId));
        }

        public void RevokeRole(RoleId roleId)
        {
            if(roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            if(!this.HasRole(roleId))
            {
                throw new InvalidOperationException("Role wasn't assumed.");
            }

            this.roles.Remove(roleId);

            EventManager.Instance.Notify(new UserRoleRevoked(this.Id, roleId));
        }
    }
}