using Identity.Core.Domain;
using Identity.Core.Events;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class RolePermissionObtainedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();

            RolePermissionObtained rolePermissionObtained = this.GetRolePermissionObtained(roleId);

            Assert.That(rolePermissionObtained.RoleId, Is.EqualTo(roleId.ToGuid()));
        }

        private RolePermissionObtained GetRolePermissionObtained(
            RoleId roleId = null,
            PermissionId obtainedPermissionId = null)
        {
            return new RolePermissionObtained(
                roleId: roleId ?? RoleId.Generate(),
                obtainedPermissionId: obtainedPermissionId ?? new PermissionId(new ResourceId("MyResource"), "Permission"));
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            PermissionId obtainedPermissionId = new(new ResourceId("MyResource"), "Permission");

            RolePermissionObtained rolePermissionObtained = this.GetRolePermissionObtained(
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(rolePermissionObtained.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId.ToString()));
        }
    }
}