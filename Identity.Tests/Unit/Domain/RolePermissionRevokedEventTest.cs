using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RolePermissionRevokedEventTest
    {
        [Test]
        public void TestConstruction_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            var roleId = RoleId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var rolePermissionRevokedEvent = new RolePermissionRevokedEvent(
                roleId: roleId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(rolePermissionRevokedEvent.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            var roleId = RoleId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var rolePermissionRevokedEvent = new RolePermissionRevokedEvent(
                roleId: roleId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(rolePermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(revokedPermissionId));
        }
    }
}