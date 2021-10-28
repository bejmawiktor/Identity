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
            RoleId assumedRoleId = RoleId.Generate();

            var userRoleAssumed = new UserRoleAssumed(
                userId: userId,
                assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumed.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenAssumedRoleIdGiven_ThenAssumedRoleIdIsSet()
        {
            UserId userId = UserId.Generate();
            RoleId assumedRoleId = RoleId.Generate();

            var userRoleAssumed = new UserRoleAssumed(
                userId: userId,
                assumedRoleId: assumedRoleId);

            Assert.That(userRoleAssumed.AssumedRoleId, Is.EqualTo(assumedRoleId));
        }
    }
}