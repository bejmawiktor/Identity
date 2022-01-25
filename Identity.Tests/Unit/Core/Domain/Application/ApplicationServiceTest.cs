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
    using Application = Identity.Core.Domain.Application;

    public class ApplicationServiceTest
    {
        private UserBuilder UserBuilder = new UserBuilder();

        [Test]
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            ApplicationService applicationService = new(
                unitOfWork);

            Assert.That(applicationService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        [Test]
        public void TestConstructor_WhenNullUnitOfWorkGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("unitOfWork"),
                () => new ApplicationService(null));
        }

        [Test]
        public async Task TestCreateApplicationAsync_WhenNoExceptionsThrown_ThenApplicationIsPersisted()
        {
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(this.UserBuilder.Build()));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepositoryMock.Object)
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            ApplicationService applicationService = new(unitOfWork);

            await applicationService.CreateApplicationAsync(
                userId: this.UserBuilder.Id,
                name: "MyApp",
                callbackUrl: new Url("http://example.com/1"),
                homepageUrl: new Url("http://example.com"));

            applicationsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Application>()), Times.Once);
        }

        [Test]
        public async Task TestCreateApplicationAsync_WhenNoExceptionsThrown_ThenApplicationCreatedIsPublished()
        {
            ApplicationCreated applicationCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(this.UserBuilder.Build()));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepositoryMock.Object)
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            ApplicationService applicationService = new(unitOfWork);

            await applicationService.CreateApplicationAsync(
                userId: this.UserBuilder.Id,
                name: "MyApp",
                callbackUrl: new Url("http://example.com/1"),
                homepageUrl: new Url("http://example.com"));

            Assert.Multiple(() =>
            {
                Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(this.UserBuilder.Id.ToGuid()));
                Assert.That(applicationCreated.ApplicationName, Is.EqualTo("MyApp"));
                Assert.That(applicationCreated.ApplicationCallbackUrl, Is.EqualTo("http://example.com/1"));
                Assert.That(applicationCreated.ApplicationHomepageUrl, Is.EqualTo("http://example.com"));
            });
        }

        [Test]
        public async Task TestCreateApplicationAsync_WhenAddApplicationThrowsException_ThenApplicationCreatedIsNotPublished()
        {
            ApplicationCreated applicationCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(this.UserBuilder.Build()));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Application>()))
                .Throws(new Exception());
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepositoryMock.Object)
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            ApplicationService applicationService = new(unitOfWork);

            try
            {
                await applicationService.CreateApplicationAsync(
                    userId: this.UserBuilder.Id,
                    name: "MyApp",
                    callbackUrl: new Url("http://example.com/1"),
                    homepageUrl: new Url("http://example.com"));
            }
            catch(Exception)
            {
            }

            Assert.That(applicationCreated, Is.Null);
        }

        [Test]
        public async Task TestCreateApplicationAsync_WhenGetUserThrowsException_ThenApplicationCreatedIsNotPublished()
        {
            ApplicationCreated applicationCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<UserId>()))
                .Throws(new Exception());
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            ApplicationService applicationService = new(unitOfWork);

            try
            {
                await applicationService.CreateApplicationAsync(
                    userId: this.UserBuilder.Id,
                    name: "MyApp",
                    callbackUrl: new Url("http://example.com/1"),
                    homepageUrl: new Url("http://example.com"));
            }
            catch(Exception)
            {
            }

            Assert.That(applicationCreated, Is.Null);
        }

        [Test]
        public void TestCreateApplicationAsync_WhenGetUserReturnsNull_ThenUserNotFoundExceptionIsThrown()
        {
            ApplicationCreated applicationCreated = null;
            Mock<IEventDispatcher> eventDispatcherMock = new();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult((User)null));
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            ApplicationService applicationService = new(unitOfWork);

            Assert.ThrowsAsync(
                Is.InstanceOf<UserNotFoundException>()
                    .And.Message
                    .EqualTo($"User {this.UserBuilder.Id} not found."),
                () => applicationService.CreateApplicationAsync(
                    userId: this.UserBuilder.Id,
                    name: "MyApp",
                    callbackUrl: new Url("http://example.com/1"),
                    homepageUrl: new Url("http://example.com")));
        }
    }
}