using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserRoleAssumedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserRoleAssumed userRoleAssumed = this.GetUserRoleAssumed(userId);

            Assert.That(userRoleAssumed.UserId, Is.EqualTo(userId));
        }

        private UserRoleAssumed GetUserRoleAssumed(
            UserId userId = null, 
            RoleId assumedRoleId = null)
        {
            return new UserRoleAssumed(
                userId: userId ?? UserId.Generate(),
                assumedRoleId: assumedRoleId ?? RoleId.Generate());
        }

        [Test]
        public void TestConstructor_WhenAssumedRoleIdGiven_ThenAssumedRoleIdIsSet()
        {
            RoleId assumedRoleId = RoleId.Generate();

            UserRoleAssumed userRoleAssumed = this.GetUserRoleAssumed(assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumed.AssumedRoleId, Is.EqualTo(assumedRoleId));
        }
    }
}