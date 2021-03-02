using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RolePermissionRevokedTest
    {
        [Test]
        public void TestConstruction_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            var roleId = RoleId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var rolePermissionRevoked = new RolePermissionRevoked(
                roleId: roleId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(rolePermissionRevoked.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            var roleId = RoleId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var rolePermissionRevoked = new RolePermissionRevoked(
                roleId: roleId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(rolePermissionRevoked.RevokedPermissionId, Is.EqualTo(revokedPermissionId));
        }
    }
}