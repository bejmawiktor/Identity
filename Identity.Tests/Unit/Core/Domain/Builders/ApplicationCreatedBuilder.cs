using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    internal class ApplicationCreatedBuilder
    {
        private static readonly ApplicationId DefaultId = ApplicationId.Generate();
        private static readonly UserId DefaultUserId = UserId.Generate();

        public ApplicationId ApplicationId { get; private set; } = ApplicationCreatedBuilder.DefaultId;
        public UserId ApplicationUserId { get; private set; } = ApplicationCreatedBuilder.DefaultUserId;
        public string ApplicationName { get; private set; } = "MyApplication";
        public Url ApplicationHomepageUrl { get; private set; } = new Url("http://wwww.example.com");
        public Url ApplicationCallbackUrl { get; private set; } = new Url("http://wwww.example.com/1");

        public ApplicationCreatedBuilder WithApplicationId(ApplicationId applicationId)
        {
            this.ApplicationId = applicationId;

            return this;
        }

        public ApplicationCreatedBuilder WithApplicationUserId(UserId applicationUserId)
        {
            this.ApplicationUserId = applicationUserId;

            return this;
        }

        public ApplicationCreatedBuilder WithApplicationName(string applicationName)
        {
            this.ApplicationName = applicationName;

            return this;
        }

        public ApplicationCreatedBuilder WithApplicationHomepageUrl(Url applicationHomepageUrl)
        {
            this.ApplicationHomepageUrl = applicationHomepageUrl;

            return this;
        }

        public ApplicationCreatedBuilder WithApplicationCallbackUrl(Url applicationCallbackUrl)
        {
            this.ApplicationCallbackUrl = applicationCallbackUrl;

            return this;
        }

        public ApplicationCreated Build()
            => new ApplicationCreated(
                applicationId: this.ApplicationId,
                applicationName: this.ApplicationName,
                applicationUserId: this.ApplicationUserId,
                applicationHomepageUrl: this.ApplicationHomepageUrl,
                applicationCallbackUrl: this.ApplicationCallbackUrl);
    }
}