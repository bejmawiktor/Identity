using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserPermissionRevokedEventTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userPermissionRevokedEvent = new UserPermissionRevokedEvent(
                userId: userId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(userPermissionRevokedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            var userId = UserId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userPermissionRevokedEvent = new UserPermissionRevokedEvent(
                userId: userId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(userPermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(revokedPermissionId));
        }
    }
}