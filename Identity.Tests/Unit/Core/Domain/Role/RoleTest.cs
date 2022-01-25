using DDD.Domain.Events;
using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
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
            Role role = new RoleBuilder()
                .WithName("Role name 2")
                .Build();

            Assert.That(role.Name, Is.EqualTo("Role name 2"));
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
            Role role = new RoleBuilder()
                .WithDescription("My role description 2.")
                .Build();

            Assert.That(role.Description, Is.EqualTo("My role description 2."));
        }

        [Test]
        public void TestNameSet_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            Role role = RoleBuilder.DefaultRole;

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("name"),
                () => role.Name = null);
        }

        [Test]
        public void TestDescriptionSet_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            Role role = RoleBuilder.DefaultRole;

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Name can't be empty."),
                () => role.Name = string.Empty);
        }

        [Test]
        public void TestDescriptionSet_WhenNameGiven_ThenNameIsSet()
        {
            Role role = RoleBuilder.DefaultRole;

            role.Name = "My role";

            Assert.That(role.Name, Is.EqualTo("My role"));
        }

        [Test]
        public void TestDescriptionSet_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            Role role = RoleBuilder.DefaultRole;

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => role.Description = null);
        }

        [Test]
        public void TestDescriptionSet_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            Role role = RoleBuilder.DefaultRole;

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => role.Description = string.Empty);
        }

        [Test]
        public void TestDescriptionSet_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            Role role = RoleBuilder.DefaultRole;

            role.Description = "My changed description.";

            Assert.That(role.Description, Is.EqualTo("My changed description."));
        }

        [Test]
        public void TestCreate_WhenCreatingRole_ThenNewRoleIsReturned()
        {
            Role role = Role.Create(
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
        public void TestCreate_WhenCreatingRole_ThenRoleCreatedIsNotified()
        {
            RoleCreated roleCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<RoleCreated>()))
                .Callback((RoleCreated p) => roleCreated = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            Role role = Role.Create(
                name: "MyRole",
                description: "My role description.");

            Assert.Multiple(() =>
            {
                Assert.That(roleCreated.RoleId, Is.EqualTo(role.Id.ToGuid()));
                Assert.That(roleCreated.RoleName, Is.EqualTo(role.Name));
                Assert.That(roleCreated.RoleDescription, Is.EqualTo(role.Description));
            });
        }

        [Test]
        public void TestObtainPermission_WhenObtainingPermission_ThenRolePermissionObtainedIsNotified()
        {
            RolePermissionObtained rolePermissionObtained = null;
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<RolePermissionObtained>()))
                .Callback((RolePermissionObtained p) => rolePermissionObtained = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Role role = RoleBuilder.DefaultRole;

            role.ObtainPermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(rolePermissionObtained.RoleId, Is.EqualTo(role.Id.ToGuid()));
                Assert.That(rolePermissionObtained.ObtainedPermissionId, Is.EqualTo(permissionId.ToString()));
            });
        }

        [Test]
        public void TestObtainPermission_WhenPermissionGiven_ThenRoleHasPermission()
        {
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            Role role = RoleBuilder.DefaultRole;

            role.ObtainPermission(permissionId);

            Assert.That(role.IsPermittedTo(permissionId), Is.True);
        }

        [Test]
        public void TestRevokePermission_WhenRovekingPermission_ThenRolePermissionRevokedIsNotified()
        {
            RolePermissionRevoked rolePermissionRevoked = null;
            PermissionId permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<RolePermissionRevoked>()))
                .Callback((RolePermissionRevoked p) => rolePermissionRevoked = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Role role = RoleBuilder.DefaultRole;
            role.ObtainPermission(permissionId);

            role.RevokePermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(rolePermissionRevoked.RoleId, Is.EqualTo(role.Id.ToGuid()));
                Assert.That(rolePermissionRevoked.RevokedPermissionId, Is.EqualTo(permissionId.ToString()));
            });
        }

        [Test]
        public void TestRevokePermission_WhenPermissionIdGiven_ThenPermissionIsRevoked()
        {
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            Role role = RoleBuilder.DefaultRole;
            role.ObtainPermission(permissionId);

            role.RevokePermission(permissionId);

            Assert.That(role.IsPermittedTo(permissionId), Is.False);
        }
    }
}