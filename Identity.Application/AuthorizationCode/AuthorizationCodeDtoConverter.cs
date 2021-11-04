using DDD.Application.Model.Converters;
using Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Application
{
    public class AuthorizationCodeDtoConverter
    : IAggregateRootDtoConverter<AuthorizationCode, AuthorizationCodeId, AuthorizationCodeDto, (Guid ApplicationId, string Code)>
    {
        public AuthorizationCodeDto ToDto(AuthorizationCode authorizationCode)
        {
            if(authorizationCode == null)
            {
                throw new ArgumentNullException(nameof(authorizationCode));
            }

            return new AuthorizationCodeDto(
                code: authorizationCode.Id.Code.ToString(),
                applicationId: authorizationCode.Id.ApplicationId.ToGuid(),
                expiresAt: authorizationCode.ExpiresAt,
                used: authorizationCode.Used,
                permissions: this.ConvertPermissions(authorizationCode.Permissions));
        }

        private IEnumerable<(string ResourceId, string Name)> ConvertPermissions(IEnumerable<PermissionId> permissions)
        {
            return permissions.Select(p => this.CreatePermissionIdTuple(p));
        }

        private (string ResourceId, string Name) CreatePermissionIdTuple(PermissionId p)
        {
            return (ResourceId: p.ResourceId.ToString(), Name: p.Name);
        }

        public (Guid ApplicationId, string Code) ToDtoIdentifier(AuthorizationCodeId authorizationCodeId)
        {
            if(authorizationCodeId == null)
            {
                throw new ArgumentNullException(nameof(authorizationCodeId));
            }

            return (authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code.ToString());
        }
    }
}