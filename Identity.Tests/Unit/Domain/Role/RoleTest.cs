using DDD.Events;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RoleTest
    {
        [Test]
        public void TestConstructor_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("name"),
                () => new Role(
                    id: RoleId.Generate(),
                    name: null,
                    description: "My role description."));
        }

        [Test]
        public void TestConstructor_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Name can't be empty."),
                () => new Role(
                    id: RoleId.Generate(),
                    name: string.Empty,
                    description: "My role description."));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.That(role.Name, Is.EqualTo("Role name"));
        }

        [Test]
        public void TestConstructor_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => new Role(
                    id: RoleId.Generate(),
                    name: "Role name",
                    description: null));
        }

        [Test]
        public void TestConstructor_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => new Role(
                    id: RoleId.Generate(),
                    name: "Role name",
                    description: string.Empty));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.That(role.Description, Is.EqualTo("My role description."));
        }

        [Test]
        public void TestNameSet_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "My role.",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("name"),
                () => role.Name = null);
        }

        [Test]
        public void TestDescriptionSet_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "My role.",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Name can't be empty."),
                () => role.Name = string.Empty);
        }

        [Test]
        public void TestDescriptionSet_WhenNameGiven_ThenNameIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            role.Name = "My role";

            Assert.That(role.Name, Is.EqualTo("My role"));
        }

        [Test]
        public void TestDescriptionSet_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => role.Description = null);
        }

        [Test]
        public void TestDescriptionSet_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => role.Description = string.Empty);
        }

        [Test]
        public void TestDescriptionSet_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            role.Description = "My changed description.";

            Assert.That(role.Description, Is.EqualTo("My changed description."));
        }

        [Test]
        public void TestCreate_WhenCreatingRole_ThenNewRoleIsReturned()
        {
            var role = Role.Create(
                name: "MyRole",
                description: "My role description.");

            Assert.Multiple(() =>
            {
                Assert.That(role.Id, Is.Not.Null);
                Assert.That(role.Name, Is.EqualTo("MyRole"));
                Assert.That(role.Description, Is.EqualTo("My role description."));
                Assert.That(role.Permissions, Is.Empty);
            });
        }

        [Test]
        public void TestCreate_WhenCreatingRole_ThenRoleCreatedEventIsNotified()
        {
            RoleCreatedEvent roleCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<RoleCreatedEvent>()))
                .Callback((RoleCreatedEvent p) => roleCreatedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            var role = Role.Create(
                name: "MyRole",
                description: "My role description.");

            Assert.Multiple(() =>
            {
                Assert.That(roleCreatedEvent.RoleId, Is.EqualTo(role.Id));
                Assert.That(roleCreatedEvent.RoleName, Is.EqualTo(role.Name));
                Assert.That(roleCreatedEvent.RoleDescription, Is.EqualTo(role.Description));
            });
        }

        [Test]
        public void TestObtainPermission_WhenObtainingPermission_ThenRolePermissionObtainedEventIsNotified()
        {
            RolePermissionObtainedEvent rolePermissionObtainedEvent = null;
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<RolePermissionObtainedEvent>()))
                .Callback((RolePermissionObtainedEvent p) => rolePermissionObtainedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var role = Role.Create(
                name: "MyRole",
                description: "My role description.");

            role.ObtainPermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(rolePermissionObtainedEvent.RoleId, Is.EqualTo(role.Id));
                Assert.That(rolePermissionObtainedEvent.ObtainedPermissionId, Is.EqualTo(permissionId));
            });
        }

        [Test]
        public void TestObtainPermission_WhenPermissionGiven_ThenRoleHasPermission()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var role = Role.Create(
                name: "MyRole",
                description: "My role description.");

            role.ObtainPermission(permissionId);

            Assert.That(role.IsPermittedTo(permissionId), Is.True);
        }

        [Test]
        public void TestRevokePermission_WhenRovekingPermission_ThenRolePermissionRevokedEventIsNotified()
        {
            RolePermissionRevokedEvent rolePermissionRevokedEvent = null;
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<RolePermissionRevokedEvent>()))
                .Callback((RolePermissionRevokedEvent p) => rolePermissionRevokedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var role = Role.Create(
                name: "MyRole",
                description: "My role description.");
            role.ObtainPermission(permissionId);

            role.RevokePermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(rolePermissionRevokedEvent.RoleId, Is.EqualTo(role.Id));
                Assert.That(rolePermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(permissionId));
            });
        }

        [Test]
        public void TestRevokePermission_WhenPermissionIdGiven_ThenPermissionIsRevoked()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var role = Role.Create(
                name: "MyRole",
                description: "My role description.");
            role.ObtainPermission(permissionId);

            role.RevokePermission(permissionId);

            Assert.That(role.IsPermittedTo(permissionId), Is.False);
        }
    }
}