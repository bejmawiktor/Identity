using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RolePermissionObtainedTest
    {
        [Test]
        public void TestConstruction_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            var roleId = RoleId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var roleCreated = new RolePermissionObtained(
                roleId: roleId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(roleCreated.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            var roleId = RoleId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var roleCreated = new RolePermissionObtained(
                roleId: roleId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(roleCreated.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId));
        }
    }
}