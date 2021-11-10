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
            PermissionCreated permissionCreated = this.GetPermissionCreated(
                permissionId: permissionId);

            Assert.That(permissionCreated.PermissionId, Is.EqualTo(permissionId));
        }

        private PermissionCreated GetPermissionCreated(
            PermissionId permissionId = null, 
            string permissionDescription = null)
        {
            var permissionIdReplacement = new PermissionId(new ResourceId("TestResource"), "MyPermission");

            return new PermissionCreated(
                permissionId: permissionId ?? permissionIdReplacement, 
                permissionDescription: permissionDescription ?? "Test permission description");
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            PermissionCreated permissionCreated = this.GetPermissionCreated(
                permissionDescription: "Test permission description");

            Assert.That(permissionCreated.PermissionDescription, Is.EqualTo("Test permission description"));
        }
    }
}