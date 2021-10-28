using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RolePermissionObtainedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var roleCreated = new RolePermissionObtained(
                roleId: roleId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(roleCreated.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            RoleId roleId = RoleId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var roleCreated = new RolePermissionObtained(
                roleId: roleId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(roleCreated.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId));
        }
    }
}