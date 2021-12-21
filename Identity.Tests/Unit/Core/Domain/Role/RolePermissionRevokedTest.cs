using Identity.Core.Domain;
using Identity.Core.Events;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class RolePermissionRevokedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();

            RolePermissionRevoked rolePermissionRevoked = this.GetRolePermissionRevoked(roleId);

            Assert.That(rolePermissionRevoked.RoleId, Is.EqualTo(roleId.ToGuid()));
        }

        private RolePermissionRevoked GetRolePermissionRevoked(
            RoleId roleId = null, 
            PermissionId permissionId = null)
        {
            return new RolePermissionRevoked(
                roleId: roleId ?? RoleId.Generate(),
                revokedPermissionId: permissionId ?? new PermissionId(new ResourceId("MyResource"), "Permission"));
        }

        [Test]
        public void TestConstructor_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            RolePermissionRevoked rolePermissionRevoked = this.GetRolePermissionRevoked(permissionId: revokedPermissionId);

            Assert.That(rolePermissionRevoked.RevokedPermissionId, Is.EqualTo(revokedPermissionId.ToString()));
        }
    }
}