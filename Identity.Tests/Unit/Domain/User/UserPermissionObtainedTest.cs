using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserPermissionObtainedTest
    {
        [Test]
        public void TestConstruction_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = UserId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userCreatedEvent = new UserPermissionObtained(
                userId: userId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(userCreatedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstruction_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            var userId = UserId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userCreatedEvent = new UserPermissionObtained(
                userId: userId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(userCreatedEvent.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId));
        }
    }
}