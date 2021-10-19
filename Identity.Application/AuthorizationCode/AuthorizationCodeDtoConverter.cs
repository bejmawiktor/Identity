using DDD.Application.Model.Converters;
using Identity.Domain;
using System;

namespace Identity.Application
{
    public class AuthorizationCodeDtoConverter
    : IAggregateRootDtoConverter<AuthorizationCode, AuthorizationCodeId, AuthorizationCodeDto, (Guid ApplicationId, string Code)>
    {
        public AuthorizationCodeDto ToDto(AuthorizationCode authorizationCode)
        {
            if (authorizationCode == null)
            {
                throw new ArgumentNullException(nameof(authorizationCode));
            }

            return new AuthorizationCodeDto(
                code: authorizationCode.Id.Code,
                applicationId: authorizationCode.Id.ApplicationId.ToGuid(),
                expiresAt: authorizationCode.ExpiresAt,
                used: authorizationCode.Used);
        }

        public (Guid ApplicationId, string Code) ToDtoIdentifier(AuthorizationCodeId authorizationCodeId)
        {
            if(authorizationCodeId == null)
            {
                throw new ArgumentNullException(nameof(authorizationCodeId));
            }

            return (authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code);
        }
    }
}
