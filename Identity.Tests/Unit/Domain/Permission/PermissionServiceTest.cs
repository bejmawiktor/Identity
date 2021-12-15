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
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            var unitOfWork = this.GetUnitOfWork();

            var permissionService = new PermissionService(unitOfWork);

            Assert.That(permissionService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        private IUnitOfWork GetUnitOfWork(
            IResourcesRepository resourcesRepository = null, 
            IPermissionsRepository permissionsRepository = null)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ResourcesRepository)
                .Returns(resourcesRepository ?? new Mock<IResourcesRepository>().Object);
            unitOfWorkMock.Setup(x => x.PermissionsRepository)
                .Returns(permissionsRepository ?? new Mock<IPermissionsRepository>().Object);
            var unitOfWork = unitOfWorkMock.Object;

            return unitOfWork;
        }

        [Test]
        public void TestConstructor_WhenNullUnitOfWorkGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("unitOfWork"),
                () => new PermissionService(null));
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
            var unitOfWork = this.GetUnitOfWork(
                resourcesRepositoryMock.Object, 
                permissionsRepositoryMock.Object);
            var permissionService = new PermissionService(unitOfWork);

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
            var unitOfWork = this.GetUnitOfWork(
                resourcesRepositoryMock.Object);
            var permissionService = new PermissionService(unitOfWork);

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
            var unitOfWork = this.GetUnitOfWork(
                resourcesRepositoryMock.Object,
                permissionsRepositoryMock.Object);
            var permissionService = new PermissionService(unitOfWork);

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
            var unitOfWork = this.GetUnitOfWork(
                resourcesRepositoryMock.Object);
            var permissionService = new PermissionService(unitOfWork);

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
            var unitOfWork = this.GetUnitOfWork(
                resourcesRepositoryMock.Object);
            var permissionService = new PermissionService(unitOfWork);

            Assert.ThrowsAsync(
                Is.InstanceOf<ResourceNotFoundException>()
                    .And.Message
                    .EqualTo($"Resource {resourceId} not found."),
                () => permissionService.CreatePermission(resourceId, "AddSomething", "Permission description."));
        }
    }
}