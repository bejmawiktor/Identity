using Identity.Core.Domain;
using Identity.Core.Events;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class UserRoleRevokedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserRoleRevoked userRoleRevoked = this.GetUserRoleRevoked(userId);

            Assert.That(userRoleRevoked.UserId, Is.EqualTo(userId.ToGuid()));
        }

        private UserRoleRevoked GetUserRoleRevoked(
            UserId userId = null,
            RoleId revokedRoleId = null)
        {
            return new UserRoleRevoked(
                userId: userId ?? UserId.Generate(),
                revokedRoleId: revokedRoleId ?? RoleId.Generate());
        }

        [Test]
        public void TestConstructor_WhenRevokedRoleIdGiven_ThenRevokedRoleIdIsSet()
        {
            RoleId revokedRoleId = RoleId.Generate();

            UserRoleRevoked userRoleRevoked = this.GetUserRoleRevoked(revokedRoleId: revokedRoleId);

            Assert.That(userRoleRevoked.RevokedRoleId, Is.EqualTo(revokedRoleId.ToGuid()));
        }
    }
}