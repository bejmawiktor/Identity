using Identity.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                name: "RoleName",
                description: "Test role description");

            Assert.That(roleCreatedEvent.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public void TestConstruction_WhenNameGiven_ThenNameIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreatedEvent = new RoleCreatedEvent(
                roleId: roleId,
                name: "RoleName",
                description: "Test role description");

            Assert.That(roleCreatedEvent.Name, Is.EqualTo("RoleName"));
        }

        [Test]
        public void TestConstruction_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var roleId = RoleId.Generate();

            var roleCreatedEvent = new RoleCreatedEvent(
                roleId: roleId,
                name: "RoleName",
                description: "Test role description");

            Assert.That(roleCreatedEvent.Description, Is.EqualTo("Test role description"));
        }
    }
}