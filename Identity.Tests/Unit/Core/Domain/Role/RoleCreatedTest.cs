using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class RoleCreatedTest
    {
        [Test]
        public void TestConstructor_WhenRoleIdGiven_ThenRoleIdIsSet()
        {
            RoleId roleId = RoleId.Generate();

            RoleCreated roleCreated = new RoleCreatedBuilder()
                .WithRoleId(roleId)
                .Build();

            Assert.That(roleCreated.RoleId, Is.EqualTo(roleId.ToGuid()));
        }

        [Test]
        public void TestConstructor_WhenRoleNameGiven_ThenRoleNameIsSet()
        {
            RoleCreated roleCreated = new RoleCreatedBuilder()
                .WithRoleName("RoleName 2")
                .Build();

            Assert.That(roleCreated.RoleName, Is.EqualTo("RoleName 2"));
        }

        [Test]
        public void TestConstructor_WhenRoleDescriptionGiven_ThenRoleDescriptionIsSet()
        {
            RoleCreated roleCreated = new RoleCreatedBuilder()
                .WithRoleDescription("Test role description 2")
                .Build();

            Assert.That(roleCreated.RoleDescription, Is.EqualTo("Test role description 2"));
        }
    }
}