using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserRoleRevokedEventTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var revokedRoleId = RoleId.Generate();

            var userRoleRevokedEvent = new UserRoleRevokedEvent(
                userId: userId,
                revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevokedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenRevokedRoleIdGiven_ThenRevokedRoleIdIsSet()
        {
            var userId = UserId.Generate();
            var revokedRoleId = RoleId.Generate();

            var userRoleRevokedEvent = new UserRoleRevokedEvent(
                userId: userId,
                revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevokedEvent.RevokedRoleId, Is.EqualTo(revokedRoleId));
        }
    }
}