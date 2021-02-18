using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class ResourceTest
    {
        [Test]
        public void TestConstruction_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var resource = new Resource(
                id: new ResourceId("TestResource"),
                description: "Test resource description");

            Assert.That(resource.Description, Is.EqualTo("Test resource description"));
        }

        [Test]
        public void TestConstruction_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => new Resource(
                    id: new ResourceId("TestResource"),
                    description: string.Empty));
        }

        [Test]
        public void TestConstruction_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => new Resource(
                    id: new ResourceId("TestResource"),
                    description: null));
        }

        [Test]
        public void TestCreatePermission_WhenCreating_ThenNewPermissionIsReturned()
        {
            var resourceId = new ResourceId("TestResource");
            var resource = new Resource(
                id: resourceId,
                description: "Test resource description");

            var permission = resource.CreatePermission(
                name: "AddPermission",
                description: "Allows users to add something.");

            Assert.Multiple(() =>
            {
                Assert.That(permission.Id, Is.EqualTo(new PermissionId(
                    resourceId: resourceId,
                    name: "AddPermission")));
                Assert.That(permission.Description, Is.EqualTo("Allows users to add something."));
            });
        }
    }
}