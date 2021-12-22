﻿using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionTest
    {
        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            Permission permission = this.GetPermission(
                description: "It allows user to grant permission to other users.");

            Assert.That(permission.Description, Is.EqualTo("It allows user to grant permission to other users."));
        }

        private Permission GetPermission(
            ResourceId resourceId = null,
            string name = null,
            string description = null)
        {
            return new Permission(
                id: new PermissionId(
                    resourceId: resourceId ?? new ResourceId("MyResource"),
                    name: name ?? "GrantPermission"),
                description: description ?? "It allows user to grant permission to other users.");
        }

        [Test]
        public void TestConstructor_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => new Permission(
                    id: new PermissionId(
                        resourceId: new ResourceId("MyResource"),
                        name: "GrantPermission"),
                    description: string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => new Permission(
                    id: new PermissionId(
                        resourceId: new ResourceId("MyResource"),
                        name: "GrantPermission"),
                    description: null));
        }
    }
}