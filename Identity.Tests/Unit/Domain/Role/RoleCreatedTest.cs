using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RoleCreatedTest
    {
        [Test]
        public void TestConstruction_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreated = new RoleCreated(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreated.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenRoleNameGiven_ThenRoleNameIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreated = new RoleCreated(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreated.RoleName, Is.EqualTo("RoleName"));
        }

        [Test]
        public void TestConstruction_WhenRoleDescriptionGiven_ThenRoleDescriptionIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreated = new RoleCreated(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreated.RoleDescription, Is.EqualTo("Test role description"));
        }
    }
}