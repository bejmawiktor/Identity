using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PermissionTest
    {
        [Test]
        public void TestConstruction_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var permission = new Permission(
                id: new PermissionId(
                    resourceId: new ResourceId("MyResource"),
                    name: "GrantPermission"),
                description: "It allows user to grant permission to other users.");

            Assert.That(permission.Description, Is.EqualTo("It allows user to grant permission to other users."));
        }

        [Test]
        public void TestConstructing_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
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
        public void TestConstructing_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
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