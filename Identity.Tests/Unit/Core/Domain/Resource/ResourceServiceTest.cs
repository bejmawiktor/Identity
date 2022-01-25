﻿using DDD.Domain.Events;
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
    public class ResourceServiceTest
    {
        [Test]
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            ResourceService resourceService = new(unitOfWork);

            Assert.That(resourceService.UnitOfWork, Is.EqualTo(unitOfWork));
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
            Mock<IResourcesRepository> resourcesRepositoryMock = new Mock<IResourcesRepository>();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .Build();
            ResourceService resourceService = new(unitOfWork);

            await resourceService.CreateResourceAsync("MyResource", "My resource description.");

            resourcesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Resource>()), Times.Once);
        }

        [Test]
        public async Task TestCreateResourceAsync_WhenNoExceptionsThrown_ThenResourceCreatedIsPublished()
        {
            ResourceCreated resourceCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreated = p as ResourceCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            ResourceService resourceService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

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
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => resourceCreated = p as ResourceCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Mock<IResourcesRepository> resourcesRepositoryMock = new();
            resourcesRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Resource>())).Throws(new Exception());
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithResourcesRepository(resourcesRepositoryMock.Object)
                .Build();
            ResourceService resourceService = new(unitOfWork);

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