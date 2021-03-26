using Identity.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class CreateResourceCommandHandlerTest
    {
        [Test]
        public void TestConstructor_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var createResourceCommandHandler = new CreateResourceCommandHandler(
                resourcesRepository,
                usersRepository,
                rolesRepository);

            Assert.That(createResourceCommandHandler.ResourcesRepository, Is.EqualTo(resourcesRepository));
        }

        [Test]
        public void TestConstructor_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var createResourceCommandHandler = new CreateResourceCommandHandler(
                resourcesRepository,
                usersRepository,
                rolesRepository);

            Assert.That(createResourceCommandHandler.UsersRepository, Is.EqualTo(usersRepository));
        }

        [Test]
        public void TestConstructor_WhenRolesRepositoryGiven_ThenRolesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var createResourceCommandHandler = new CreateResourceCommandHandler(
                resourcesRepository,
                usersRepository,
                rolesRepository);

            Assert.That(createResourceCommandHandler.RolesRepository, Is.EqualTo(rolesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("resourcesRepository"),
               () => new CreateResourceCommandHandler(null, usersRepository, rolesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new CreateResourceCommandHandler(resourcesRepository, null, rolesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullRolesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("rolesRepository"),
               () => new CreateResourceCommandHandler(resourcesRepository, usersRepository, null));
        }

        [Test]
        public void TestHandle_WhenUserIsNotAuthorizedToCreateResource_ThenUnauthorizedAccessExceptionIsThrown()
        {
            var userId = Guid.NewGuid();
            var user = new UserDto(
                id: userId,
                email: "example@example.com",
                hashedPassword: Identity.Domain.HashedPassword.Hash("MyPassword").ToString());
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            usersRepositoryMock.Setup(u => u.Get(It.IsAny<Guid>())).Returns(user);
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var createResourceCommandHandler = new CreateResourceCommandHandler(
                resourcesRepository,
                usersRepository,
                rolesRepository);
            var createResourceCommand = new CreateResourceCommand(
                "MyResource",
                "My resource description.",
                userId);

            Assert.Throws(
               Is.InstanceOf<Identity.Application.UnauthorizedAccessException>()
                   .And.Message
                   .EqualTo("User isn't authorized to create resource."),
               () => createResourceCommandHandler.Handle(createResourceCommand));
        }

        [Test]
        public void TestHandleAsync_WhenUserIsNotAuthorizedToCreateResource_ThenUnauthorizedAccessExceptionIsThrown()
        {
            var userId = Guid.NewGuid();
            var user = new UserDto(
                id: userId,
                email: "example@example.com",
                hashedPassword: Identity.Domain.HashedPassword.Hash("MyPassword").ToString());
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            usersRepositoryMock.Setup(u => u.Get(It.IsAny<Guid>())).Returns(user);
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var createResourceCommandHandler = new CreateResourceCommandHandler(
                resourcesRepository,
                usersRepository,
                rolesRepository);
            var createResourceCommand = new CreateResourceCommand(
                "MyResource",
                "My resource description.",
                userId);

            Assert.Throws(
               Is.InstanceOf<Identity.Application.UnauthorizedAccessException>()
                   .And.Message
                   .EqualTo("User isn't authorized to create resource."),
               () => createResourceCommandHandler.HandleAsync(createResourceCommand).Wait());
        }
    }
}