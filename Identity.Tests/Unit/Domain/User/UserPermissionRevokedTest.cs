using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserPermissionRevokedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserPermissionRevoked userPermissionRevokedEvent = this.GetUserPermissionRevoked(
                userId: userId);

            Assert.That(userPermissionRevokedEvent.UserId, Is.EqualTo(userId));
        }

        private UserPermissionRevoked GetUserPermissionRevoked(
            UserId userId = null, 
            PermissionId revokedPermissionId = null)
        {
            return new UserPermissionRevoked(
                userId ?? UserId.Generate(),
                revokedPermissionId ?? new PermissionId(new ResourceId("MyResource"), "Permission"));  
        }

        [Test]
        public void TestConstructor_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            var revokedPermissionId = new PermissionId(new ResourceId("MyResource"), "Permission");

            UserPermissionRevoked userPermissionRevokedEvent = this.GetUserPermissionRevoked(
                revokedPermissionId: revokedPermissionId);

            Assert.That(userPermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(revokedPermissionId));
        }
    }
}