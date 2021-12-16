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
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();

            var resourceService = new ResourceService(unitOfWork);

            Assert.That(resourceService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        private IUnitOfWork GetUnitOfWork(IResourcesRepository resourcesRepository = null)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ResourcesRepository)
                .Returns(resourcesRepository ?? new Mock<IResourcesRepository>().Object);
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
               () => new ResourceService(unitOfWork: null));
        }

        [Test]
        public async Task TestCreateResourceAsync_WhenNoExceptionsThrown_ThenResourceIsPersisted()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            IUnitOfWork unitOfWork = this.GetUnitOfWork(resourcesRepositoryMock.Object);
            var resourceService = new ResourceService(unitOfWork);

            await resourceService.CreateResourceAsync("MyResource", "My resource description.");

            resourcesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Resource>()), Times.Once);
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
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            var resourceService = new ResourceService(unitOfWork);

            await resourceService.CreateResourceAsync("MyResource", "My resource description.");

            Assert.Multiple(() =>
            {
                Assert.That(resourceCreated.ResourceId, Is.EqualTo("MyResource"));
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
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            resourcesRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Resource>())).Throws(new Exception());
            IUnitOfWork unitOfWork = this.GetUnitOfWork(resourcesRepositoryMock.Object);
            var resourceService = new ResourceService(unitOfWork);

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