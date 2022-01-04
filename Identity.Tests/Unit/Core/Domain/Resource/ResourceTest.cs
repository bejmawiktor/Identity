using DDD.Domain.Events;
using Identity.Core.Domain;
using Identity.Core.Events;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class ResourceTest
    {
        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            Resource resource = this.GetResource(
                description: "Test resource description");

            Assert.That(resource.Description, Is.EqualTo("Test resource description"));
        }

        private Resource GetResource(
            ResourceId id = null,
            string description = null)
        {
            return new Resource(
                id: id ?? new ResourceId("TestResource"),
                description: description ?? "Test resource description");
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
            ResourceId resourceId = new ResourceId("TestResource");
            Resource resource = this.GetResource(resourceId);

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
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<PermissionCreated>()))
                .Callback((PermissionCreated p) => permissionCreated = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            ResourceId resourceId = new("TestResource");
            Resource resource = this.GetResource(resourceId);

            Permission permission = resource.CreatePermission(
                name: "AddPermission",
                description: "Allows users to add something.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreated.PermissionId, Is.EqualTo(permission.Id.ToString()));
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
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<ResourceCreated>()))
                .Callback((ResourceCreated p) => resourceCreated = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            Resource resource = Resource.Create(
                name: "MyResource",
                description: "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreated.ResourceId, Is.EqualTo(resource.Id.ToString()));
                Assert.That(resourceCreated.ResourceDescription, Is.EqualTo(resource.Description));
            });
        }
    }
}