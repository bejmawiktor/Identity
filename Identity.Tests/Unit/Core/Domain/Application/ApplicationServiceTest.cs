using DDD.Domain.Events;
using Identity.Core.Domain;
using Identity.Core.Events;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Core.Domain
{
    using Application = Identity.Core.Domain.Application;

    public class ApplicationServiceTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();

            ApplicationService applicationService = new(
                unitOfWork);

            Assert.That(applicationService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        private IUnitOfWork GetUnitOfWork(
            IApplicationsRepository applicationsRepository = null,
            IUsersRepository usersRepository = null)
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(x => x.ApplicationsRepository)
                .Returns(applicationsRepository ?? new Mock<IApplicationsRepository>().Object);
            unitOfWorkMock.Setup(x => x.UsersRepository)
                .Returns(usersRepository ?? new Mock<IUsersRepository>().Object);
            IUnitOfWork unitOfWork = unitOfWorkMock.Object;

            return unitOfWork;
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
            UserId userId = UserId.Generate();
            User user = new(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepositoryMock.Object,
                usersRepositoryMock.Object);
            ApplicationService applicationService = new(unitOfWork);

            await applicationService.CreateApplicationAsync(
                userId: userId,
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
            UserId userId = UserId.Generate();
            User user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepositoryMock.Object,
                usersRepositoryMock.Object);
            ApplicationService applicationService = new(unitOfWork);

            await applicationService.CreateApplicationAsync(
                userId: userId,
                name: "MyApp",
                callbackUrl: new Url("http://example.com/1"),
                homepageUrl: new Url("http://example.com"));

            Assert.Multiple(() =>
            {
                Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(userId.ToGuid()));
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
            UserId userId = UserId.Generate();
            User user = new(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Application>()))
                .Throws(new Exception());
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepositoryMock.Object,
                usersRepositoryMock.Object);
            ApplicationService applicationService = new(unitOfWork);

            try
            {
                await applicationService.CreateApplicationAsync(
                    userId: userId,
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
            UserId userId = UserId.Generate();
            User user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<UserId>()))
                .Throws(new Exception());
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: usersRepositoryMock.Object);
            ApplicationService applicationService = new(unitOfWork);

            try
            {
                await applicationService.CreateApplicationAsync(
                    userId: userId,
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
            UserId userId = UserId.Generate();
            User user = new(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult((User)null));
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: usersRepositoryMock.Object);
            ApplicationService applicationService = new(unitOfWork);

            Assert.ThrowsAsync(
                Is.InstanceOf<UserNotFoundException>()
                    .And.Message
                    .EqualTo($"User {userId} not found."),
                () => applicationService.CreateApplicationAsync(
                    userId: userId,
                    name: "MyApp",
                    callbackUrl: new Url("http://example.com/1"),
                    homepageUrl: new Url("http://example.com")));
        }
    }
}