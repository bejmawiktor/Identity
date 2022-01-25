using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class RolePermissionRevokedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();

            RolePermissionRevoked rolePermissionRevoked = new RolePermissionRevokedBuilder()
                .WithRoleId(roleId)
                .Build();

            Assert.That(rolePermissionRevoked.RoleId, Is.EqualTo(roleId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenRevokedPermissionIdGiven_ThenRevokedPermissionIdIsSet()
        {
            PermissionId revokedPermissionId = new(new ResourceId("MyResource"), "Permission");

            RolePermissionRevoked rolePermissionRevoked = new RolePermissionRevokedBuilder()
                .WithRevokedPermissionId(revokedPermissionId)
                .Build();

            Assert.That(rolePermissionRevoked.RevokedPermissionId, Is.EqualTo(revokedPermissionId.ToString()));
        }
    }
}