using Identity.Core.Application;
using Identity.Core.Domain;
using System;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    internal class ApplicationDtoBuilder
    {
        private static readonly string DefaultSecretKey = EncryptedSecretKey
            .Encrypt(Identity.Core.Domain.SecretKey.Generate())
            .ToString();

        private static readonly Guid DefaultId = Guid.NewGuid();
        private static readonly Guid DefaultUserId = Guid.NewGuid();

        public static ApplicationDto DefaultApplicationDto
            => new ApplicationDtoBuilder().Build();

        public Guid Id { get; private set; } = ApplicationDtoBuilder.DefaultId;
        public Guid UserId { get; private set; } = ApplicationDtoBuilder.DefaultUserId;
        public string Name { get; private set; } = "MyApplication";
        public string SecretKey { get; private set; } = ApplicationDtoBuilder.DefaultSecretKey;
        public string HomepageUrl { get; private set; } = "https://www.example.com";
        public string CallbackUrl { get; private set; } = "https://www.example.com/1";

        public ApplicationDtoBuilder WithId(Guid id)
        {
            this.Id = id;

            return this;
        }

        public ApplicationDtoBuilder WithUserId(Guid userId)
        {
            this.UserId = userId;

            return this;
        }

        public ApplicationDtoBuilder WithName(string name)
        {
            this.Name = name;

            return this;
        }

        public ApplicationDtoBuilder WithSecretKey(string secretKey)
        {
            this.SecretKey = secretKey;

            return this;
        }

        public ApplicationDtoBuilder WithHompageUrl(string homepageUrl)
        {
            this.HomepageUrl = homepageUrl;

            return this;
        }

        public ApplicationDtoBuilder WithCallbackUrl(string callbackUrl)
        {
            this.CallbackUrl = callbackUrl;

            return this;
        }

        public ApplicationDto Build()
            => new ApplicationDto(
                this.Id,
                this.UserId,
                this.Name,
                this.SecretKey,
                this.HomepageUrl,
                this.CallbackUrl);
    }
}