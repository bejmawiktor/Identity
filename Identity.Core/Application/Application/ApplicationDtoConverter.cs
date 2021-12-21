using DDD.Application.Model.Converters;
using System;

namespace Identity.Core.Application
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class ApplicationDtoConverter : IAggregateRootDtoConverter<Application, ApplicationId, ApplicationDto, Guid>
    {
        public ApplicationDto ToDto(Application application)
        {
            if(application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return new ApplicationDto(
                id: application.Id.ToGuid(),
                userId: application.UserId.ToGuid(),
                name: application.Name,
                secretKey: application.SecretKey.ToString(),
                homepageUrl: application.HomepageUrl.ToString(),
                callbackUrl: application.CallbackUrl.ToString());
        }

        public Guid ToDtoIdentifier(ApplicationId applicationId)
        {
            if(applicationId == null)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            return applicationId.ToGuid();
        }
    }
}