using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RolePermissionObtainedEventTest
    {
        [Test]
        public void TestConstruction_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            var roleId = RoleId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var roleCreatedEvent = new RolePermissionObtainedEvent(
                roleId: roleId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(roleCreatedEvent.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            var roleId = RoleId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var roleCreatedEvent = new RolePermissionObtainedEvent(
                roleId: roleId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(roleCreatedEvent.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId));
        }
    }
}