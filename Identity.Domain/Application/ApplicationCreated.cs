using DDD.Domain.Events;

namespace Identity.Domain
{
    public class ApplicationCreated : Event
    {
        public ApplicationId ApplicationId { get; }
        public UserId ApplicationUserId { get; }
        public string ApplicationName { get; }
        public Url ApplicationHomepageUrl { get; }
        public Url ApplicationCallbackUrl { get; }

        public ApplicationCreated(
            ApplicationId applicationId,
            UserId applicationUserId,
            string applicationName,
            Url applicationHomepageUrl,
            Url applicationCallbackUrl)
        {
            this.ApplicationId = applicationId;
            this.ApplicationUserId = applicationUserId;
            this.ApplicationName = applicationName;
            this.ApplicationHomepageUrl = applicationHomepageUrl;
            this.ApplicationCallbackUrl = applicationCallbackUrl;
        }
    }
}