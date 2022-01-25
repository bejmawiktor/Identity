using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class UserPermissionObtainedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserPermissionObtained userPermissionObtained = new UserPermissionObtainedBuilder()
                .WithUserId(userId)
                .Build();

            Assert.That(userPermissionObtained.UserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            PermissionId obtainedPermissionId = new(new ResourceId("MyResource2"), "Permission2");

            UserPermissionObtained userPermissionObtained = new UserPermissionObtainedBuilder()
                .WithObtainedPermissionId(obtainedPermissionId)
                .Build();

            Assert.That(userPermissionObtained.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId.ToString()));
        }
    }
}