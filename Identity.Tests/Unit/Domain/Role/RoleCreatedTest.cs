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

            RoleCreated roleCreated = this.GetRoleCreated(roleId);

            Assert.That(roleCreated.RoleId, Is.EqualTo(roleId.ToGuid()));
        }

        private RoleCreated GetRoleCreated(
            RoleId roleId = null, 
            string roleName = null, 
            string roleDescription = null)
        {
            return new RoleCreated(
                roleId: roleId ?? RoleId.Generate(),
                roleName: roleName ?? "RoleName",
                roleDescription: roleDescription ?? "Test role description");
        }

        [Test]
        public void TestConstructor_WhenRoleNameGiven_ThenRoleNameIsSet()
        {
            RoleCreated roleCreated = this.GetRoleCreated(roleName: "RoleName");

            Assert.That(roleCreated.RoleName, Is.EqualTo("RoleName"));
        }

        [Test]
        public void TestConstructor_WhenRoleDescriptionGiven_ThenRoleDescriptionIsSet()
        {
            RoleCreated roleCreated = this.GetRoleCreated(roleDescription: "Test role description");

            Assert.That(roleCreated.RoleDescription, Is.EqualTo("Test role description"));
        }
    }
}