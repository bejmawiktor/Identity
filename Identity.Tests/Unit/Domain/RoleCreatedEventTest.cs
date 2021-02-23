using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RoleCreatedEventTest
    {
        [Test]
        public void TestConstruction_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreatedEvent = new RoleCreatedEvent(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreatedEvent.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenRoleNameGiven_ThenRoleNameIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreatedEvent = new RoleCreatedEvent(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreatedEvent.RoleName, Is.EqualTo("RoleName"));
        }

        [Test]
        public void TestConstruction_WhenRoleDescriptionGiven_ThenRoleDescriptionIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreatedEvent = new RoleCreatedEvent(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreatedEvent.RoleDescription, Is.EqualTo("Test role description"));
        }
    }
}