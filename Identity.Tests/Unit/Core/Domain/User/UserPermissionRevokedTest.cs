using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class UserPermissionRevokedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserPermissionRevoked userPermissionRevokedEvent = new UserPermissionRevokedBuilder()
                .WithUserId(userId)
                .Build();

            Assert.That(userPermissionRevokedEvent.UserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            PermissionId revokedPermissionId = new(new ResourceId("MyResource"), "Permission");

            UserPermissionRevoked userPermissionRevokedEvent = new UserPermissionRevokedBuilder()
                .WithRevokedPermissionId(revokedPermissionId)
                .Build();

            Assert.That(userPermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(revokedPermissionId.ToString()));
        }
    }
}