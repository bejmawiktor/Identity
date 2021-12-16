using DDD.Domain.Events;
using System;

namespace Identity.Domain
{
    public class ApplicationCreated : Event
    {
        public Guid ApplicationId { get; }
        public Guid ApplicationUserId { get; }
        public string ApplicationName { get; }
        public string ApplicationHomepageUrl { get; }
        public string ApplicationCallbackUrl { get; }

        public ApplicationCreated(
            ApplicationId applicationId,
            UserId applicationUserId,
            string applicationName,
            Url applicationHomepageUrl,
            Url applicationCallbackUrl)
        {
            this.ApplicationId = applicationId.ToGuid();
            this.ApplicationUserId = applicationUserId.ToGuid();
            this.ApplicationName = applicationName;
            this.ApplicationHomepageUrl = applicationHomepageUrl.ToString();
            this.ApplicationCallbackUrl = applicationCallbackUrl.ToString();
        }
    }
}