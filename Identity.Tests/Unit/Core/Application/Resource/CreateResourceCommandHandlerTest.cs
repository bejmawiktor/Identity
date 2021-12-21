using Identity.Core.Application;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Application
{
    using Password = Identity.Core.Domain.Password;
    using UnauthorizedAccessException = Identity.Core.Application.UnauthorizedAccessException;

    [TestFixture]
    public class CreateResourceCommandHandlerTest
    {
        [Test]
        public void TestConstructor_WhenNullUnitOfWorkGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("unitOfWork"),
               () => new CreateResourceCommandHandler(null));
        }

        [Test]
        public void TestHandleAsync_WhenUserIsNotAuthorizedToCreateResource_ThenUnauthorizedAccessExceptionIsThrown()
        {
            Guid userId = Guid.NewGuid();
            var user = new UserDto(
                id: userId,
                email: "example@example.com",
                hashedPassword: Identity.Core.Domain.HashedPassword.Hash(new Password("MyPassword")).ToString());
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            CreateResourceCommandHandler createResourceCommandHandler = new CreateResourceCommandHandler(
                this.GetUnitOfWorkMock(usersRepository: usersRepository).Object);
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

        private Mock<IUnitOfWork> GetUnitOfWorkMock(
            IApplicationsRepository applicationsRepository = null,
            IAuthorizationCodesRepository authorizationCodesRepository = null,
            IPermissionsRepository permissionsRepository = null,
            IResourcesRepository resourcesRepository = null,
            IRolesRepository rolesRepository = null,
            IRefreshTokensRepository refreshTokensRepository = null,
            IUsersRepository usersRepository = null)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ApplicationsRepository)
                .Returns(applicationsRepository ?? new Mock<IApplicationsRepository>().Object);
            unitOfWorkMock.Setup(x => x.AuthorizationCodesRepository)
                .Returns(authorizationCodesRepository ?? new Mock<IAuthorizationCodesRepository>().Object);
            unitOfWorkMock.Setup(x => x.PermissionsRepository)
                .Returns(permissionsRepository ?? new Mock<IPermissionsRepository>().Object);
            unitOfWorkMock.Setup(x => x.ResourcesRepository)
                .Returns(resourcesRepository ?? new Mock<IResourcesRepository>().Object);
            unitOfWorkMock.Setup(x => x.RolesRepository)
                .Returns(rolesRepository ?? new Mock<IRolesRepository>().Object);
            unitOfWorkMock.Setup(x => x.RefreshTokensRepository)
                .Returns(refreshTokensRepository ?? new Mock<IRefreshTokensRepository>().Object);
            unitOfWorkMock.Setup(x => x.UsersRepository)
                .Returns(usersRepository ?? new Mock<IUsersRepository>().Object);

            return unitOfWorkMock;
        }
    }
}