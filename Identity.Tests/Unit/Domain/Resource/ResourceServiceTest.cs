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
        public void TestCreateResource_WhenNoExceptionsThrown_ThenResourceCreatedIsPublished()
        {
            ResourceCreated resourceCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreated = p as ResourceCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var repositoryMock = new Mock<IResourcesRepository>();
            var resourceService = new ResourceService(repositoryMock.Object);

            resourceService.CreateResource("MyResource", "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreated.ResourceId, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resourceCreated.ResourceDescription, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestCreateResource_WhenExceptionsThrown_ThenResourceCreatedIsNotPublished()
        {
            ResourceCreated resourceCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreated = p as ResourceCreated);
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

            Assert.That(resourceCreated, Is.Null);
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
        public async Task TestCreateResourceAsync_WhenNoExceptionsThrown_ThenResourceCreatedIsPublished()
        {
            ResourceCreated resourceCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreated = p as ResourceCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var repositoryMock = new Mock<IResourcesRepository>();
            var resourceService = new ResourceService(repositoryMock.Object);

            await resourceService.CreateResourceAsync("MyResource", "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreated.ResourceId, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resourceCreated.ResourceDescription, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public async Task TestCreateResourceAsync_WhenExceptionsThrown_ThenResourceCreatedIsNotPublished()
        {
            ResourceCreated resourceCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreated = p as ResourceCreated);
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

            Assert.That(resourceCreated, Is.Null);
        }
    }
}