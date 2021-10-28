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

            var userCreatedEvent = new UserCreated(
                userId: userId,
                userEmail: new EmailAddress("example@example.com"));

            Assert.That(userCreatedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenUserEmailGiven_ThenUserEmailIsSet()
        {
            UserId userId = UserId.Generate();

            var userCreatedEvent = new UserCreated(
                userId: userId,
                userEmail: new EmailAddress("example@example.com"));

            Assert.That(userCreatedEvent.UserEmail, Is.EqualTo(new EmailAddress("example@example.com")));
        }
    }
}