using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class UserCreatedBuilder
    {
        private static readonly UserId DefaultUserId = UserId.Generate();

        public UserId UserId { get; private set; } = UserCreatedBuilder.DefaultUserId;
        public EmailAddress UserEmailAddress { get; private set; } = new EmailAddress("example@example.com");

        public UserCreatedBuilder WithUserId(UserId userId)
        {
            this.UserId = userId;

            return this;
        }

        public UserCreatedBuilder WithUserEmailAddress(EmailAddress userEmailAddress)
        {
            this.UserEmailAddress = userEmailAddress;

            return this;
        }

        public UserCreated Build()
            => new UserCreated(
                this.UserId, 
                this.UserEmailAddress);
    }
}