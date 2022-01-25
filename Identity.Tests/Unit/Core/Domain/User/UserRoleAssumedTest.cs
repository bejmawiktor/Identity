using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class UserRoleAssumedTest
    {
        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            UserId userId = UserId.Generate();

            UserRoleAssumed userRoleAssumed = new UserRoleAssumedBuilder()
                .WithUserId(userId)
                .Build();

            Assert.That(userRoleAssumed.UserId, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenAssumedRoleIdGiven_ThenAssumedRoleIdIsSet()
        {
            RoleId assumedRoleId = RoleId.Generate();

            UserRoleAssumed userRoleAssumed = new UserRoleAssumedBuilder()
                .WithAssumedRoleId(assumedRoleId)
                .Build();

            Assert.That(userRoleAssumed.AssumedRoleId, Is.EqualTo(assumedRoleId.ToGuid()));
        }
    }
}