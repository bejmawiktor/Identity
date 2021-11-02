using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain
{
    public class AuthorizationCode : AggregateRoot<AuthorizationCodeId>
    {
        public DateTime ExpiresAt { get; }
        public bool Used { get; }
        public IEnumerable<PermissionId> Permissions { get; }
        private int SecondsToExpire => 60;

        public AuthorizationCode(
            AuthorizationCodeId id,
            DateTime expiresAt,
            bool used,
            IEnumerable<PermissionId> permissions)
        : base(id)
        {
            this.ValidateMembers(permissions);

            this.ExpiresAt = expiresAt;
            this.Used = used;
            this.Permissions = permissions;
        }

        private void ValidateMembers(IEnumerable<PermissionId> permissions)
        {
            if(permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            if(permissions.Count() == 0)
            {
                throw new ArgumentException("Can't create authorization code without permissions.");
            }
        }

        internal AuthorizationCode(
            AuthorizationCodeId id,
            IEnumerable<PermissionId> permissions)
        : base(id)
        {
            this.ValidateMembers(permissions);

            this.ExpiresAt = DateTime.Now.AddSeconds(this.SecondsToExpire);
            this.Used = false;
            this.Permissions = permissions;
        }
    }
}