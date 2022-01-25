using DDD.Domain.Events;
using Identity.Core.Application;
using Identity.Tests.Unit.Core.Application.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Core.Application
{
    using Password = Identity.Core.Domain.Password;
    using UnauthorizedAccessException = Identity.Core.Application.UnauthorizedAccessException;

    internal class CreateResourceCommandHandlerTest
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
            UserDto user = new(
                id: userId,
                email: "example@example.com",
                hashedPassword: Identity.Core.Domain.HashedPassword.Hash(new Password("MyPassword")).ToString());
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .Build();
            CreateResourceCommandHandler createResourceCommandHandler = new(unitOfWork);
            CreateResourceCommand createResourceCommandM = new(
                "MyResource",
                "My resource description.",
                userId);

            UnauthorizedAccessException exception = Assert.ThrowsAsync<UnauthorizedAccessException>(
                async () => await createResourceCommandHandler.Handle(createResourceCommandM, CancellationToken.None));

            Assert.That(exception, Is.InstanceOf<UnauthorizedAccessException>()
                .And.Message
                .EqualTo("User isn't authorized to create resource."));
        }

        [Test]
        public async Task TestHandleAsync_WhenUserIsAuthorizedToCreateResource_ThenResourceIsCreated()
        {
            Guid userId = Guid.NewGuid();
            UserDto user = new UserDto(
                id: userId,
                email: "example@example.com",
                hashedPassword: Identity.Core.Domain.HashedPassword.Hash(new Password("MyPassword")).ToString(),
                permissions: new (string ResourceId, string Name)[]
                {
                    new PermissionDtoConverter().ToDtoIdentifier(Permissions.CreateResource.Id)
                });
            Mock<IUsersRepository> usersRepositoryMock = new();
            Mock<IResourcesRepository> resourcesRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .WithResourcesRepository(resourcesRepository)
                .Build();
            CreateResourceCommandHandler createResourceCommandHandler = new(unitOfWork);
            EventManager.Instance.EventDispatcher = new Mock<IEventDispatcher>().Object;
            CreateResourceCommand createResourceCommandM = new(
                "MyResource",
                "My resource description.",
                userId);

            await createResourceCommandHandler.Handle(createResourceCommandM, CancellationToken.None);

            resourcesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ResourceDto>()), Times.Once);
        }
    }
}