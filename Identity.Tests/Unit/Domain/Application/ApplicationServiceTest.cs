using DDD.Domain.Events;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Domain
{
    using Application = Identity.Domain.Application;

    public class ApplicationServiceTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructor_WhenApplicationsRepositoryGiven_ThenApplicationsRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;

            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

            Assert.That(applicationService.ApplicationsRepository, Is.EqualTo(applicationsRepository));
        }

        [Test]
        public void TestConstructor_WhenNullApplicationsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("applicationsRepository"),
                () => new ApplicationService(
                    applicationsRepository: null,
                    usersRepository: usersRepositoryMock.Object));
        }

        [Test]
        public void TestConstructor_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;

            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

            Assert.That(applicationService.UsersRepository, Is.EqualTo(usersRepository));
        }

        [Test]
        public void TestConstructor_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("usersRepository"),
                () => new ApplicationService(
                    applicationsRepository: applicationsRepositoryMock.Object,
                    usersRepository: null));
        }

        [Test]
        public async Task TestCreateApplicationAsync_WhenNoExceptionsThrown_ThenApplicationIsPersisted()
        {
            UserId userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

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
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            UserId userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

            await applicationService.CreateApplicationAsync(
                userId: userId,
                name: "MyApp",
                callbackUrl: new Url("http://example.com/1"),
                homepageUrl: new Url("http://example.com"));

            Assert.Multiple(() =>
            {
                Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(userId));
                Assert.That(applicationCreated.ApplicationName, Is.EqualTo("MyApp"));
                Assert.That(applicationCreated.ApplicationCallbackUrl, Is.EqualTo(new Url("http://example.com/1")));
                Assert.That(applicationCreated.ApplicationHomepageUrl, Is.EqualTo(new Url("http://example.com")));
            });
        }

        [Test]
        public async Task TestCreateApplicationAsync_WhenAddApplicationThrowsException_ThenApplicationCreatedIsNotPublished()
        {
            ApplicationCreated applicationCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            UserId userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Application>()))
                .Throws(new Exception());
            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

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
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            UserId userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<UserId>()))
                .Throws(new Exception());
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

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
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<IEvent>()))
                .Callback((IEvent p) => applicationCreated = p as ApplicationCreated);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            UserId userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("example@example.com"),
                password: TestPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(p => p.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult((User)null));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var applicationService = new ApplicationService(
                applicationsRepository: applicationsRepositoryMock.Object,
                usersRepository: usersRepositoryMock.Object);

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