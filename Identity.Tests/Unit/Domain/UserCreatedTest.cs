using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserCreatedEventTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();

            var userCreatedEvent = new UserCreatedEvent(
                userId: userId,
                userEmail: new EmailAddress("example@example.com"));

            Assert.That(userCreatedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenUserEmailGiven_ThenUserEmailIsSet()
        {
            var userId = UserId.Generate();

            var userCreatedEvent = new UserCreatedEvent(
                userId: userId,
                userEmail: new EmailAddress("example@example.com"));

            Assert.That(userCreatedEvent.UserEmail, Is.EqualTo(new EmailAddress("example@example.com")));
        }
    }
}