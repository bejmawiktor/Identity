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

            UserPermissionObtained userPermissionObtained = this.GetUserPermissionObtained(userId);

            Assert.That(userPermissionObtained.UserId, Is.EqualTo(userId.ToGuid()));
        }

        private UserPermissionObtained GetUserPermissionObtained(
            UserId userId = null,
            PermissionId obtainedPermissionId = null)
        {
            return new UserPermissionObtained(
                userId ?? UserId.Generate(),
                obtainedPermissionId ?? new PermissionId(new ResourceId("MyResource"), "Permission"));
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            var obtainedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            UserPermissionObtained userPermissionObtained = this.GetUserPermissionObtained(
                obtainedPermissionId: obtainedPermissionId);

            Assert.That(userPermissionObtained.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId.ToString()));
        }
    }
}