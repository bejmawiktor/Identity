using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain
{
    internal class TokenInformation : ValueObject
    {
        public Guid Id { get; }
        public ApplicationId ApplicationId { get; }
        public TokenType TokenType { get; }
        public DateTime ExpirationDate { get; }
        public IReadOnlyCollection<PermissionId> Permissions { get; }

        public TokenInformation(
            Guid id,
            ApplicationId applicationId,
            TokenType tokenType,
            IEnumerable<PermissionId> permissions,
            DateTime? expirationDate = null)
        {
            this.Id = id;
            this.TokenType = tokenType;
            this.ApplicationId = applicationId;
            this.ExpirationDate = expirationDate ?? tokenType.GenerateExpirationDate();
            this.Permissions = permissions.ToList().AsReadOnly();
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Id;
            yield return this.ApplicationId;
            yield return this.TokenType;
            yield return this.ExpirationDate.ToString("G");

            foreach(PermissionId permissionId in this.Permissions)
            {
                yield return permissionId;
            }
        }
    }
}