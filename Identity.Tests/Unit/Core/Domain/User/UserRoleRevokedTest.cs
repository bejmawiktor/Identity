using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
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

            UserRoleRevoked userRoleRevoked = new UserRoleRevokedBuilder()
                .WithUserId(userId)
                .Build();

            Assert.That(userRoleRevoked.UserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenRevokedRoleIdGiven_ThenRevokedRoleIdIsSet()
        {
            RoleId revokedRoleId = RoleId.Generate();

            UserRoleRevoked userRoleRevoked = new UserRoleRevokedBuilder()
                .WithRevokedRoleId(revokedRoleId)
                .Build();

            Assert.That(userRoleRevoked.RevokedRoleId, Is.EqualTo(revokedRoleId.ToGuid()));
        }
    }
}