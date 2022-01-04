using DDD.Application.Model;
using Identity.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Application
{
    using ApplicationId = Domain.ApplicationId;

    public class AuthorizationCodeDto : IAggregateRootDto<AuthorizationCode, AuthorizationCodeId>
    {
        public string Code { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }
        public IEnumerable<(string ResourceId, string Name)> Permissions { get; set; }

        public AuthorizationCodeDto(
            string code,
            Guid applicationId,
            DateTime expiresAt,
            bool used,
            IEnumerable<(string ResourceId, string Name)> permissions)
        {
            this.Code = code;
            this.ApplicationId = applicationId;
            this.ExpiresAt = expiresAt;
            this.Used = used;
            this.Permissions = permissions;
        }

        AuthorizationCode IDomainObjectDto<AuthorizationCode>.ToDomainObject()
            => this.ToAuthorizationCode();

        internal AuthorizationCode ToAuthorizationCode()
            => new AuthorizationCode(
                new AuthorizationCodeId(new HashedCode(this.Code), new ApplicationId(this.ApplicationId)),
                this.ExpiresAt,
                this.Used,
                this.ConvertPermissions());

        private IEnumerable<PermissionId> ConvertPermissions()
           => this.Permissions.Select(p => this.CreatePermissionId(p));

        private PermissionId CreatePermissionId((string ResourceId, string Name) permissionIdTuple)
            => new PermissionId(new ResourceId(permissionIdTuple.ResourceId), permissionIdTuple.Name);

        public override bool Equals(object obj)
        {
            return obj is AuthorizationCodeDto dto
                && this.Code == dto.Code
                && this.ApplicationId.Equals(dto.ApplicationId)
                && this.ExpiresAt == dto.ExpiresAt
                && this.Used == dto.Used
                && this.Permissions.SequenceEqual(dto.Permissions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + this.Code.GetHashCode();
                hash = hash * 23 + this.ApplicationId.GetHashCode();
                hash = hash * 23 + this.ExpiresAt.GetHashCode();
                hash = hash * 23 + this.Used.GetHashCode();

                foreach((string ResourceId, string Name) permission in this.Permissions)
                {
                    hash = hash * 23 + permission.GetHashCode();
                }

                return hash;
            }
        }
    }
}