using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PermissionCreatedTest
    {
        [Test]
        public void TestConstructor_WhenPermissionIdGiven_ThenPermissionIdIsSet()
        {
            var permissionId = new PermissionId(new ResourceId("TestResource"), "MyPermission");
            var permissionCreated = new PermissionCreated(
                permissionId: permissionId,
                permissionDescription: "Test permission description");

            Assert.That(permissionCreated.PermissionId, Is.EqualTo(permissionId));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var permissionCreated = new PermissionCreated(
                permissionId: new PermissionId(new ResourceId("TestResource"), "MyPermission"),
                permissionDescription: "Test permission description");

            Assert.That(permissionCreated.PermissionDescription, Is.EqualTo("Test permission description"));
        }
    }
}