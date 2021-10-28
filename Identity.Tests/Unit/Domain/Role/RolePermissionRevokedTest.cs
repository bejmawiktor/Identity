using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RolePermissionRevokedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var rolePermissionRevoked = new RolePermissionRevoked(
                roleId: roleId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(rolePermissionRevoked.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstructor_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            RoleId roleId = RoleId.Generate();
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var rolePermissionRevoked = new RolePermissionRevoked(
                roleId: roleId,
                revokedPermissionId: revokedPermissionId);

            Assert.That(rolePermissionRevoked.RevokedPermissionId, Is.EqualTo(revokedPermissionId));
        }
    }
}