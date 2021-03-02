using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserRoleAssumedTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var assumedRoleId = RoleId.Generate();

            var userRoleAssumed = new UserRoleAssumed(
                userId: userId,
                assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumed.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenAssumedRoleIdGiven_ThenAssumedRoleIdIsSet()
        {
            var userId = UserId.Generate();
            var assumedRoleId = RoleId.Generate();

            var userRoleAssumed = new UserRoleAssumed(
                userId: userId,
                assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumed.AssumedRoleId, Is.EqualTo(assumedRoleId));
        }
    }
}