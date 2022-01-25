using Identity.Core.Domain;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    using Application = Identity.Core.Domain.Application;

    internal class ApplicationBuilder
    {
        private static readonly EncryptedSecretKey DefaultSecretKey = EncryptedSecretKey.Encrypt(Identity.Core.Domain.SecretKey.Generate());
        private static readonly ApplicationId DefaultId = ApplicationId.Generate();
        private static readonly UserId DefaultUserId = UserId.Generate();

        public static Application DefaultApplication => new ApplicationBuilder().Build();

        public ApplicationId Id { get; private set; } = ApplicationBuilder.DefaultId;
        public UserId UserId { get; private set; } = ApplicationBuilder.DefaultUserId;
        public string Name { get; private set; } = "MyApplication";
        public EncryptedSecretKey SecretKey { get; private set; } = ApplicationBuilder.DefaultSecretKey;
        public Url HomepageUrl { get; private set; } = new Url("https://www.example.com");
        public Url CallbackUrl { get; private set; } = new Url("https://www.example.com/1");

        public ApplicationBuilder WithId(ApplicationId id)
        {
            this.Id = id;

            return this;
        }

        public ApplicationBuilder WithUserId(UserId userId)
        {
            this.UserId = userId;

            return this;
        }

        public ApplicationBuilder WithName(string name)
        {
            this.Name = name;

            return this;
        }

        public ApplicationBuilder WithSecretKey(EncryptedSecretKey secretKey)
        {
            this.SecretKey = secretKey;

            return this;
        }

        public ApplicationBuilder WithHompageUrl(Url homepageUrl)
        {
            this.HomepageUrl = homepageUrl;

            return this;
        }

        public ApplicationBuilder WithCallbackUrl(Url callbackUrl)
        {
            this.CallbackUrl = callbackUrl;

            return this;
        }

        public Application Build()
            => new Application(
                this.Id,
                this.UserId,
                this.Name,
                this.SecretKey,
                this.HomepageUrl,
                this.CallbackUrl);
    }
}