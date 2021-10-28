using DDD.Domain.Events;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PermissionServiceTest
    {
        [Test]
        public void TestConstructor_WhenPermissionsRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            IPermissionsRepository permissionsRepository = permissionsRepositoryMock.Object;

            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            Assert.That(permissionService.PermissionsRepository, Is.EqualTo(permissionsRepository));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionsRepository"),
                () => new PermissionService(
                    permissionsRepository: null,
                    resourcesRepository: resourcesRepositoryMock.Object));
        }

        [Test]
        public void TestConstructor_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;

            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            Assert.That(permissionService.ResourcesRepository, Is.EqualTo(resourcesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("resourcesRepository"),
                () => new PermissionService(
                    permissionsRepository: permissionsRepositoryMock.Object,
                    resourcesRepository: null));
        }

        [Test]
        public async Task TestCreatePermission_WhenNoExceptionsThrown_ThenPermissionIsPersisted()
        {
            var resourceId = new ResourceId("MyResource");
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            await permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");

            permissionsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Permission>()), Times.Once);
        }

        [Test]
        public async Task TestCreatePermission_WhenNoExceptionsThrown_ThenPermissionCreatedIsPublished()
        {
            PermissionCreated permissionCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            await permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreated.PermissionId, Is.EqualTo(new PermissionId(resourceId, "AddSomething")));
                Assert.That(permissionCreated.PermissionDescription, Is.EqualTo("Permission description."));
            });
        }

        [Test]
        public async Task TestCreatePermission_WhenAddPermissionThrowsException_ThenPermissionCreatedIsNotPublished()
        {
            PermissionCreated permissionCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            permissionsRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Permission>()))
                .Throws(new Exception());
            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            try
            {
                await permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");
            }
            catch(Exception)
            {
            }

            Assert.That(permissionCreated, Is.Null);
        }

        [Test]
        public async Task TestCreatePermission_WhenGetResourceThrowsException_ThenPermissionCreatedIsNotPublished()
        {
            PermissionCreated permissionCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<ResourceId>()))
                .Throws(new Exception());
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            try
            {
                await permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");
            }
            catch(Exception)
            {
            }

            Assert.That(permissionCreated, Is.Null);
        }

        [Test]
        public void TestCreatePermission_WhenGetResourceReturnsNull_ThenResourceNotFoundExceptionIsThrown()
        {
            PermissionCreated permissionCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult((Resource)null));
            var permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            var permissionService = new PermissionService(
                permissionsRepository: permissionsRepositoryMock.Object,
                resourcesRepository: resourcesRepositoryMock.Object);

            Assert.ThrowsAsync(
                Is.InstanceOf<ResourceNotFoundException>()
                    .And.Message
                    .EqualTo($"Resource {resourceId} not found."),
                () => permissionService.CreatePermission(resourceId, "AddSomething", "Permission description."));
        }
    }
}