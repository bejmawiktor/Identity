using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RoleCreatedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();

            var roleCreated = new RoleCreated(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreated.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstructor_WhenRoleNameGiven_ThenRoleNameIsSet()
        {
            RoleId roleId = RoleId.Generate();

            var roleCreated = new RoleCreated(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreated.RoleName, Is.EqualTo("RoleName"));
        }

        [Test]
        public void TestConstructor_WhenRoleDescriptionGiven_ThenRoleDescriptionIsSet()
        {
            RoleId roleId = RoleId.Generate();

            var roleCreated = new RoleCreated(
                roleId: roleId,
                roleName: "RoleName",
                roleDescription: "Test role description");

            Assert.That(roleCreated.RoleDescription, Is.EqualTo("Test role description"));
        }
    }
}