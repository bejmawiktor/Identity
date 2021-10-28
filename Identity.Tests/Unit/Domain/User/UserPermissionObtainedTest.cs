using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserPermissionObtainedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userCreatedEvent = new UserPermissionObtained(
                userId: userId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(userCreatedEvent.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            UserId userId = UserId.Generate();
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            var userCreatedEvent = new UserPermissionObtained(
                userId: userId,
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(userCreatedEvent.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId));
        }
    }
}