using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class RolePermissionObtainedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();

            RolePermissionObtained rolePermissionObtained = new RolePermissionObtainedBuilder()
                .WithRoleId(roleId)
                .Build();

            Assert.That(rolePermissionObtained.RoleId, Is.EqualTo(roleId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenObtainedPermissionIdGiven_ThenObtainedPermissionIdIsSet()
        {
            PermissionId obtainedPermissionId = new(new ResourceId("MyResource"), "Permission");

            RolePermissionObtained rolePermissionObtained = new RolePermissionObtainedBuilder()
                .WithObtainedPermissionId(obtainedPermissionId)
                .Build();

            Assert.That(rolePermissionObtained.ObtainedPermissionId, Is.EqualTo(obtainedPermissionId.ToString()));
        }
    }
}