using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserRoleRevokedTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var revokedRoleId = RoleId.Generate();

            var userRoleRevoked = new UserRoleRevoked(
                userId: userId,
                revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevoked.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenRevokedRoleIdGiven_ThenRevokedRoleIdIsSet()
        {
            var userId = UserId.Generate();
            var revokedRoleId = RoleId.Generate();

            var userRoleRevoked = new UserRoleRevoked(
                userId: userId,
                revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevoked.RevokedRoleId, Is.EqualTo(revokedRoleId));
        }
    }
}