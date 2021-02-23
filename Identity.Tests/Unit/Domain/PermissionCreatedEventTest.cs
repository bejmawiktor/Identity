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
    public class PermissionCreatedEventTest
    {
        [Test]
        public void TestConstruction_WhenPermissionIdGiven_ThenPermissionIdIsSet()
        {
            var permissionId = new PermissionId(new ResourceId("TestResource"), "MyPermission");
            var permissionCreatedEvent = new PermissionCreatedEvent(
                permissionId: permissionId,
                permissionDescription: "Test permission description");

            Assert.That(permissionCreatedEvent.PermissionId, Is.EqualTo(permissionId));
        }

        [Test]
        public void TestConstruction_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var permissionCreatedEvent = new PermissionCreatedEvent(
                permissionId: new PermissionId(new ResourceId("TestResource"), "MyPermission"),
                permissionDescription: "Test permission description");

            Assert.That(permissionCreatedEvent.PermissionDescription, Is.EqualTo("Test permission description"));
        }
    }
}