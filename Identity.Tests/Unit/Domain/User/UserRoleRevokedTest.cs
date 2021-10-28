using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserRoleRevokedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();
            RoleId revokedRoleId = RoleId.Generate();

            var userRoleRevoked = new UserRoleRevoked(
                userId: userId,
                revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevoked.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenRevokedRoleIdGiven_ThenRevokedRoleIdIsSet()
        {
            UserId userId = UserId.Generate();
            RoleId revokedRoleId = RoleId.Generate();

            var userRoleRevoked = new UserRoleRevoked(
                userId: userId,
                revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevoked.RevokedRoleId, Is.EqualTo(revokedRoleId));
        }
    }
}