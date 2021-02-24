using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserRoleAssumedEventTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var assumedRoleId = RoleId.Generate();

            var userRoleAssumedEvent = new UserRoleAssumedEvent(
                userId: userId,
                assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenAssumedRoleIdGiven_ThenAssumedRoleIdIsSet()
        {
            var userId = UserId.Generate();
            var assumedRoleId = RoleId.Generate();

            var userRoleAssumedEvent = new UserRoleAssumedEvent(
                userId: userId,
                assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumedEvent.AssumedRoleId, Is.EqualTo(assumedRoleId));
        }
    }
}