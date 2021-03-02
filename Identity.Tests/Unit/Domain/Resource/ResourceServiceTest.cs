using DDD.Domain.Events;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class ResourceServiceTest
    {
        [Test]
        public void TestConstructing_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var repositoryMock = new Mock<IResourcesRepository>();
            var repository = repositoryMock.Object;

            var resourceService = new ResourceService(repository);

            Assert.That(resourceService.ResourcesRepository, Is.EqualTo(repository));
        }

        [Test]
        public void TestConstructing_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("resourcesRepository"),
               () => new ResourceService(null));
        }

        [Test]
        public void TestCreateResource_WhenNoExceptionsThrown_ThenResourceIsPersisted()
        {
            var repositoryMock = new Mock<IResourcesRepository>();
            var resourceService = new ResourceService(repositoryMock.Object);

            resourceService.CreateResource("MyResource", "My resource description.");

            repositoryMock.Verify(r => r.Add(It.IsAny<Resource>()), Times.Once);
        }

        [Test]
        public void TestCreateResource_WhenNoExceptionsThrown_ThenResourceCreatedEventIsPublished()
        {
            ResourceCreatedEvent resourceCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreatedEvent = p as ResourceCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var repositoryMock = new Mock<IResourcesRepository>();
            var resourceService = new ResourceService(repositoryMock.Object);

            resourceService.CreateResource("MyResource", "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreatedEvent.ResourceId, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resourceCreatedEvent.ResourceDescription, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestCreateResource_WhenExceptionsThrown_ThenResourceCreatedEventIsNotPublished()
        {
            ResourceCreatedEvent resourceCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreatedEvent = p as ResourceCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var repositoryMock = new Mock<IResourcesRepository>();
            repositoryMock.Setup(r => r.Add(It.IsAny<Resource>())).Throws(new Exception());
            var resourceService = new ResourceService(repositoryMock.Object);

            try
            {
                resourceService.CreateResource("MyResource", "My resource description.");
            }
            catch(Exception)
            {
            }

            Assert.That(resourceCreatedEvent, Is.Null);
        }

        [Test]
        public async Task TestCreateResourceAsync_WhenNoExceptionsThrown_ThenResourceIsPersisted()
        {
            var repositoryMock = new Mock<IResourcesRepository>();
            var resourceService = new ResourceService(repositoryMock.Object);

            await resourceService.CreateResourceAsync("MyResource", "My resource description.");

            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Resource>()), Times.Once);
        }

        [Test]
        public async Task TestCreateResourceAsync_WhenNoExceptionsThrown_ThenResourceCreatedEventIsPublished()
        {
            ResourceCreatedEvent resourceCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreatedEvent = p as ResourceCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var repositoryMock = new Mock<IResourcesRepository>();
            var resourceService = new ResourceService(repositoryMock.Object);

            await resourceService.CreateResourceAsync("MyResource", "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreatedEvent.ResourceId, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resourceCreatedEvent.ResourceDescription, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public async Task TestCreateResourceAsync_WhenExceptionsThrown_ThenResourceCreatedEventIsNotPublished()
        {
            ResourceCreatedEvent resourceCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreatedEvent = p as ResourceCreatedEvent);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var repositoryMock = new Mock<IResourcesRepository>();
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Resource>())).Throws(new Exception());
            var resourceService = new ResourceService(repositoryMock.Object);

            try
            {
                await resourceService.CreateResourceAsync("MyResource", "My resource description.");
            }
            catch(Exception)
            {
            }

            Assert.That(resourceCreatedEvent, Is.Null);
        }
    }
}