using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserCreatedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserCreated userCreatedEvent = this.GetUserCreated(userId: userId);

            Assert.That(userCreatedEvent.UserId, Is.EqualTo(userId.ToGuid()));
        }

        private UserCreated GetUserCreated(
            UserId userId = null, 
            EmailAddress userEmailAddress = null)
        {
            return new UserCreated(
                userId: userId ?? UserId.Generate(),
                userEmail: userEmailAddress ?? new EmailAddress("example@example.com"));
        }

        [Test]
        public void TestConstructor_WhenUserEmailGiven_ThenUserEmailIsSet()
        {
            UserCreated userCreatedEvent = this.GetUserCreated(userEmailAddress: new EmailAddress("example@example.com"));

            Assert.That(userCreatedEvent.UserEmail, Is.EqualTo("example@example.com"));
        }
    }
}