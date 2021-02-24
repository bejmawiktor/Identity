using DDD.Events;
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
        public void TestConstructing_WhenPermissionRepositoryGiven_ThenResourceRepositoryIsSet()
        {
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            IPermissionRepository permissionRepository = permissionRepositoryMock.Object;

            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            Assert.That(permissionService.PermissionRepository, Is.EqualTo(permissionRepository));
        }

        [Test]
        public void TestConstructing_WhenNullPermissionRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            var permissionRepositoryMock = new Mock<IPermissionRepository>();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionRepository"),
                () => new PermissionService(
                    permissionRepository: null,
                    resourceRepository: resourceRepositoryMock.Object));
        }

        [Test]
        public void TestConstructing_WhenResourceRepositoryGiven_ThenResourceRepositoryIsSet()
        {
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            IResourceRepository resourceRepository = resourceRepositoryMock.Object;

            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            Assert.That(permissionService.ResourceRepository, Is.EqualTo(resourceRepository));
        }

        [Test]
        public void TestConstructing_WhenNullResourceRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            var permissionRepositoryMock = new Mock<IPermissionRepository>();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("resourceRepository"),
                () => new PermissionService(
                    permissionRepository: permissionRepositoryMock.Object,
                    resourceRepository: null));
        }

        [Test]
        public void TestCreatePermission_WhenNoExceptionsThrown_ThenPermissionIsPersisted()
        {
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(r => r.Get(It.IsAny<ResourceId>()))
                .Returns(new Resource(resourceId, "Resrouce description."));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");

            permissionRepositoryMock.Verify(r => r.Add(It.IsAny<Permission>()), Times.Once);
        }

        [Test]
        public void TestCreatePermission_WhenNoExceptionsThrown_ThenPermissionCreatedEventIsPublished()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(r => r.Get(It.IsAny<ResourceId>()))
                .Returns(new Resource(resourceId, "Resrouce description."));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreatedEvent.PermissionId, Is.EqualTo(new PermissionId(resourceId, "AddSomething")));
                Assert.That(permissionCreatedEvent.PermissionDescription, Is.EqualTo("Permission description."));
            });
        }

        [Test]
        public void TestCreatePermission_WhenAddPermissionThrowsException_ThenResourceCreatedEventIsNotPublished()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(r => r.Get(It.IsAny<ResourceId>()))
                .Returns(new Resource(resourceId, "Resrouce description."));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            permissionRepositoryMock
                .Setup(p => p.Add(It.IsAny<Permission>()))
                .Throws(new Exception());
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            try
            {
                permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");
            }
            catch(Exception)
            {
            }

            Assert.That(permissionCreatedEvent, Is.Null);
        }

        [Test]
        public void TestCreatePermission_WhenGetResourceThrowsException_ThenResourceCreatedEventIsNotPublished()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(p => p.Get(It.IsAny<ResourceId>()))
                .Throws(new Exception());
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            try
            {
                permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");
            }
            catch(Exception)
            {
            }

            Assert.That(permissionCreatedEvent, Is.Null);
        }

        [Test]
        public void TestCreatePermission_WhenGetResourceReturnsNull_ThenResourceNotFoundExceptionIsThrown()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(p => p.Get(It.IsAny<ResourceId>()))
                .Returns((Resource)null);
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            Assert.Throws(
                Is.InstanceOf<ResourceNotFoundException>()
                    .And.Message
                    .EqualTo($"Resource {resourceId} not found."),
                () => permissionService.CreatePermission(resourceId, "AddSomething", "Permission description."));
        }

        [Test]
        public async Task TestCreatePermissionAsync_WhenNoExceptionsThrown_ThenPermissionIsPersisted()
        {
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            await permissionService.CreatePermissionAsync(resourceId, "AddSomething", "Permission description.");

            permissionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Permission>()), Times.Once);
        }

        [Test]
        public async Task TestCreatePermissionAsync_WhenNoExceptionsThrown_ThenPermissionCreatedEventIsPublished()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            await permissionService.CreatePermissionAsync(resourceId, "AddSomething", "Permission description.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreatedEvent.PermissionId, Is.EqualTo(new PermissionId(resourceId, "AddSomething")));
                Assert.That(permissionCreatedEvent.PermissionDescription, Is.EqualTo("Permission description."));
            });
        }

        [Test]
        public async Task TestCreatePermissionAsync_WhenAddPermissionThrowsException_ThenResourceCreatedEventIsNotPublished()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            permissionRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Permission>()))
                .Throws(new Exception());
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            try
            {
                await permissionService.CreatePermissionAsync(resourceId, "AddSomething", "Permission description.");
            }
            catch(Exception)
            {
            }

            Assert.That(permissionCreatedEvent, Is.Null);
        }

        [Test]
        public async Task TestCreatePermissionAsync_WhenGetResourceThrowsException_ThenResourceCreatedEventIsNotPublished()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<ResourceId>()))
                .Throws(new Exception());
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            try
            {
                await permissionService.CreatePermissionAsync(resourceId, "AddSomething", "Permission description.");
            }
            catch(Exception)
            {
            }

            Assert.That(permissionCreatedEvent, Is.Null);
        }

        [Test]
        public async Task TestCreatePermissionAsync_WhenGetResourceReturnsNull_ThenResourceNotFoundExceptionIsThrown()
        {
            PermissionCreatedEvent permissionCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreatedEvent = p as PermissionCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var resourceId = new ResourceId("MyResource");
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult((Resource)null));
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            var permissionService = new PermissionService(
                permissionRepository: permissionRepositoryMock.Object,
                resourceRepository: resourceRepositoryMock.Object);

            Assert.ThrowsAsync(
                Is.InstanceOf<ResourceNotFoundException>()
                    .And.Message
                    .EqualTo($"Resource {resourceId} not found."),
                () => permissionService.CreatePermissionAsync(resourceId, "AddSomething", "Permission description."));
        }
    }
}