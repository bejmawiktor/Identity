using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserPermissionRevokedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userPermissionRevokedEvent = new UserPermissionRevoked(
                userId: userId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(userPermissionRevokedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            UserId userId = UserId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userPermissionRevokedEvent = new UserPermissionRevoked(
                userId: userId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(userPermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(revokedPermissionId));
        }
    }
}