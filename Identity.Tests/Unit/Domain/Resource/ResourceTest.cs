using DDD.Domain.Events;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class ResourceTest
    {
        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var resource = new Resource(
                id: new ResourceId("TestResource"),
                description: "Test resource description");

            Assert.That(resource.Description, Is.EqualTo("Test resource description"));
        }

        [Test]
        public void TestConstructor_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
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
        public void TestConstructor_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
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

            Permission permission = resource.CreatePermission(
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

        [Test]
        public void TestCreatePermission_WhenCreating_ThenPermissionCreatedIsNotified()
        {
            PermissionCreated permissionCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<PermissionCreated>()))
                .Callback((PermissionCreated p) => permissionCreated = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("TestResource");
            var resource = new Resource(
                id: resourceId,
                description: "Test resource description");

            Permission permission = resource.CreatePermission(
                name: "AddPermission",
                description: "Allows users to add something.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreated.PermissionId, Is.EqualTo(permission.Id));
                Assert.That(permissionCreated.PermissionDescription, Is.EqualTo(permission.Description));
            });
        }

        [Test]
        public void TestCreate_WhenCreatingResource_ThenNewResourceIsReturned()
        {
            Resource resource = Resource.Create(
                name: "MyResource",
                description: "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resource.Description, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestCreate_WhenCreatingResource_ThenResourceCreatedIsNotified()
        {
            ResourceCreated resourceCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<ResourceCreated>()))
                .Callback((ResourceCreated p) => resourceCreated = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            Resource resource = Resource.Create(
                name: "MyResource",
                description: "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreated.ResourceId, Is.EqualTo(resource.Id));
                Assert.That(resourceCreated.ResourceDescription, Is.EqualTo(resource.Description));
            });
        }
    }
}