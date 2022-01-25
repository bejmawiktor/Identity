using DDD.Domain.Events;
using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionServiceTest
    {
        [Test]
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            PermissionService permissionService = new PermissionService(unitOfWork);

            Assert.That(permissionService.UnitOfWork, Is.EqualTo(unitOfWork));
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
            ResourceId resourceId = new ResourceId("MyResource");
            Mock<IResourcesRepository> resourcesRepositoryMock = new();
            resourcesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            Mock<IPermissionsRepository> permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .WithPermissionsRepository(permissionsRepositoryMock.Object)
                .Build();
            PermissionService permissionService = new PermissionService(unitOfWork);

            await permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");

            permissionsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Permission>()), Times.Once);
        }

        [Test]
        public async Task TestCreatePermission_WhenNoExceptionsThrown_ThenPermissionCreatedIsPublished()
        {
            PermissionCreated permissionCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            ResourceId resourceId = new ResourceId("MyResource");
            Mock<IResourcesRepository> resourcesRepositoryMock = new();
            resourcesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .Build();
            PermissionService permissionService = new PermissionService(unitOfWork);

            await permissionService.CreatePermission(resourceId, "AddSomething", "Permission description.");

            Assert.Multiple(() =>
            {
                Assert.That(permissionCreated.PermissionId, Is.EqualTo(new PermissionId(resourceId, "AddSomething").ToString()));
                Assert.That(permissionCreated.PermissionDescription, Is.EqualTo("Permission description."));
            });
        }

        [Test]
        public async Task TestCreatePermission_WhenAddPermissionThrowsException_ThenPermissionCreatedIsNotPublished()
        {
            PermissionCreated permissionCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            ResourceId resourceId = new ResourceId("MyResource");
            Mock<IResourcesRepository> resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult(new Resource(resourceId, "Resource description.")));
            Mock<IPermissionsRepository> permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            permissionsRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Permission>()))
                .Throws(new Exception());
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .WithPermissionsRepository(permissionsRepositoryMock.Object)
                .Build();
            PermissionService permissionService = new PermissionService(unitOfWork);

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
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            ResourceId resourceId = new("MyResource");
            Mock<IResourcesRepository> resourcesRepositoryMock = new();
            resourcesRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<ResourceId>()))
                .Throws(new Exception());
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .Build();
            PermissionService permissionService = new(unitOfWork);

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
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => permissionCreated = p as PermissionCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            ResourceId resourceId = new("MyResource");
            Mock<IResourcesRepository> resourcesRepositoryMock = new();
            resourcesRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<ResourceId>()))
                .Returns(Task.FromResult((Resource)null));
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .Build();
            PermissionService permissionService = new(unitOfWork);

            Assert.ThrowsAsync(
                Is.InstanceOf<ResourceNotFoundException>()
                    .And.Message
                    .EqualTo($"Resource {resourceId} not found."),
                () => permissionService.CreatePermission(resourceId, "AddSomething", "Permission description."));
        }
    }
}