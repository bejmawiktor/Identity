using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class UserCreatedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserCreated userCreatedEvent = new UserCreatedBuilder()
                .WithUserId(userId)
                .Build();

            Assert.That(userCreatedEvent.UserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenUserEmailGiven_ThenUserEmailIsSet()
        {
            UserCreated userCreatedEvent = new UserCreatedBuilder()
                .WithUserEmailAddress(new EmailAddress("example@example.com"))
                .Build();

            Assert.That(userCreatedEvent.UserEmail, Is.EqualTo("example@example.com"));
        }
    }
}