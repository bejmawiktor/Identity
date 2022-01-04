﻿using Identity.Core.Domain;
using Identity.Core.Events;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionCreatedTest
    {
        [Test]
        public void TestConstructor_WhenPermissionIdGiven_ThenPermissionIdIsSet()
        {
            PermissionId permissionId = new(new ResourceId("TestResource"), "MyPermission");
            PermissionCreated permissionCreated = this.GetPermissionCreated(
                permissionId: permissionId);

            Assert.That(permissionCreated.PermissionId, Is.EqualTo(permissionId.ToString()));
        }

        private PermissionCreated GetPermissionCreated(
            PermissionId permissionId = null,
            string permissionDescription = null)
        {
            PermissionId permissionIdReplacement = new(new ResourceId("TestResource"), "MyPermission");

            return new PermissionCreated(
                permissionId: permissionId ?? permissionIdReplacement,
                permissionDescription: permissionDescription ?? "Test permission description");
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            PermissionCreated permissionCreated = this.GetPermissionCreated(
                permissionDescription: "Test permission description");

            Assert.That(permissionCreated.PermissionDescription, Is.EqualTo("Test permission description"));
        }
    }
}