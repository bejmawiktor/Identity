using Identity.Application;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Application
{
    using Password = Identity.Domain.Password;
    using UnauthorizedAccessException = Identity.Application.UnauthorizedAccessException;

    [TestFixture]
    public class CreateResourceCommandHandlerTest
    {
        [Test]
        public void TestConstructor_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;

            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(resourcesRepository);

            Assert.That(createResourceCommandHandler.ResourcesRepository, Is.EqualTo(resourcesRepository));
        }

        private CreateResourceCommandHandler GetCreateResourceCommandHandler(
            IResourcesRepository resourcesRepository = null,
            IUsersRepository usersRepository = null,
            IRolesRepository rolesRepository = null,
            IApplicationsRepository applicationsRepository = null,
            IAuthorizationCodesRepository authorizationCodesRepository = null,
            IRefreshTokensRepository refreshTokensRepository = null)
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();

            return new CreateResourceCommandHandler(
                resourcesRepository ?? resourcesRepositoryMock.Object,
                usersRepository ?? usersRepositoryMock.Object,
                rolesRepository ?? rolesRepositoryMock.Object,
                applicationsRepository ?? applicationsRepositoryMock.Object,
                authorizationCodesRepository ?? authorizationCodesRepositoryMock.Object,
                refreshTokensRepository ?? refreshTokensRepositoryMock.Object);
        }

        [Test]
        public void TestConstructor_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;

            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(
                usersRepository: usersRepository);

            Assert.That(createResourceCommandHandler.UsersRepository, Is.EqualTo(usersRepository));
        }

        [Test]
        public void TestConstructor_WhenRolesRepositoryGiven_ThenRolesRepositoryIsSet()
        {
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;

            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(
                rolesRepository: rolesRepository);

            Assert.That(createResourceCommandHandler.RolesRepository, Is.EqualTo(rolesRepository));
        }

        [Test]
        public void TestConstructor_WhenApplicationsRepositoryGiven_ThenApplicationsRepositoryIsSet()
        {
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;

            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(
                applicationsRepository: applicationsRepository);

            Assert.That(createResourceCommandHandler.ApplicationsRepository, Is.EqualTo(applicationsRepository));
        }

        [Test]
        public void TestConstructor_WhenAuthorizationCodesRepositoryGiven_ThenAuthorizationCodesRepositoryIsSet()
        {
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(
                authorizationCodesRepository: authorizationCodesRepository);

            Assert.That(createResourceCommandHandler.AuthorizationCodesRepository, Is.EqualTo(authorizationCodesRepository));
        }

        [Test]
        public void TestConstructor_WhenRefreshTokensRepositoryGiven_ThenRefreshTokensRepositoryIsSet()
        {
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;

            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(
                refreshTokensRepository: refreshTokensRepository);

            Assert.That(createResourceCommandHandler.RefreshTokensRepository, Is.EqualTo(refreshTokensRepository));
        }

        [Test]
        public void TestConstructor_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("resourcesRepository"),
               () => new CreateResourceCommandHandler(
                   null,
                   usersRepository,
                   rolesRepository,
                   applicationsRepository,
                   authorizationCodesRepository,
                   refreshTokensRepository));
        }

        [Test]
        public void TestConstructor_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new CreateResourceCommandHandler(
                   resourcesRepository,
                   null,
                   rolesRepository,
                   applicationsRepository,
                   authorizationCodesRepository,
                   refreshTokensRepository));
        }

        [Test]
        public void TestConstructor_WhenNullRolesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("rolesRepository"),
               () => new CreateResourceCommandHandler(
                   resourcesRepository,
                   usersRepository,
                   null,
                   applicationsRepository,
                   authorizationCodesRepository,
                   refreshTokensRepository));
        }

        [Test]
        public void TestConstructor_WhenNullApplicationsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("applicationsRepository"),
               () => new CreateResourceCommandHandler(
                   resourcesRepository,
                   usersRepository,
                   rolesRepository,
                   null,
                   authorizationCodesRepository,
                   refreshTokensRepository));
        }

        [Test]
        public void TestConstructor_WhenNullAuthorizationCodesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("authorizationCodesRepository"),
               () => new CreateResourceCommandHandler(
                   resourcesRepository,
                   usersRepository,
                   rolesRepository,
                   applicationsRepository,
                   null,
                   refreshTokensRepository));
        }

        [Test]
        public void TestConstructor_WhenNullRefreshTokensRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("refreshTokensRepository"),
               () => new CreateResourceCommandHandler(
                   resourcesRepository,
                   usersRepository,
                   rolesRepository,
                   applicationsRepository,
                   authorizationCodesRepository,
                   null));
        }

        [Test]
        public void TestHandleAsync_WhenUserIsNotAuthorizedToCreateResource_ThenUnauthorizedAccessExceptionIsThrown()
        {
            Guid userId = Guid.NewGuid();
            var user = new UserDto(
                id: userId,
                email: "example@example.com",
                hashedPassword: Identity.Domain.HashedPassword.Hash(new Password("MyPassword")).ToString());
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            CreateResourceCommandHandler createResourceCommandHandler = this.GetCreateResourceCommandHandler(
                usersRepository: usersRepository);
            var createResourceCommand = new CreateResourceCommand(
                "MyResource",
                "My resource description.",
                userId);

            UnauthorizedAccessException exception = Assert.ThrowsAsync<UnauthorizedAccessException>(
                async () => await createResourceCommandHandler.HandleAsync(createResourceCommand));

            Assert.That(exception, Is.InstanceOf<UnauthorizedAccessException>()
                .And.Message
                .EqualTo("User isn't authorized to create resource."));
        }
    }
}