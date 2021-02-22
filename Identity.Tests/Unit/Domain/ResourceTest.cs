using DDD.Events;
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

        [Test]
        public void TestCreatePermission_WhenCreating_ThenPermissionCreatedEventIsNotified()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<PermissionCreatedEvent>()))
                .Callback((PermissionCreatedEvent p) => permissionCreatedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("TestResource");
            var resource = new Resource(
                id: resourceId,
                description: "Test resource description");

            var permission = resource.CreatePermission(
                name: "AddPermission",
                description: "Allows users to add something.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreatedEvent.PermissionId, Is.EqualTo(permission.Id));
                Assert.That(permissionCreatedEvent.Description, Is.EqualTo(permission.Description));
            });
        }

        [Test]
        public void TestCreate_WhenCreatingResource_ThenNewResourceIsReturned()
        {
            var resource = Resource.Create(
                name: "MyResource",
                description: "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resource.Description, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestCreate_WhenCreatingResource_ThenResourceCreatedEventIsNotified()
        {
            ResourceCreatedEvent resourceCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<ResourceCreatedEvent>()))
                .Callback((ResourceCreatedEvent p) => resourceCreatedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            var resource = Resource.Create(
                name: "MyResource",
                description: "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreatedEvent.ResourceId, Is.EqualTo(resource.Id));
                Assert.That(resourceCreatedEvent.Description, Is.EqualTo(resource.Description));
            });
        }
    }
}