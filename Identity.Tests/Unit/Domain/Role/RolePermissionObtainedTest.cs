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
           
            RolePermissionObtained rolePermissionObtained = this.GetRolePermissionObtained(roleId);

            Assert.That(rolePermissionObtained.RoleId, Is.EqualTo(roleId));
        }

        private RolePermissionObtained GetRolePermissionObtained(
            RoleId roleId = null, 
            PermissionId obtainedPermissionId = null)
        {
            return new RolePermissionObtained(
                roleId: roleId, 
                obtainedPermissionId: obtainedPermissionId);
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            RolePermissionObtained rolePermissionObtained = this.GetRolePermissionObtained(
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(rolePermissionObtained.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId));
        }
    }
}