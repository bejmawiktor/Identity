using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        [Test]
        public void TestConstructor_WhenNullUnitOfWorkGiven_ThenArgumentNullIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("unitOfWork"),
               () => new AuthenticationService(unitOfWork: null));
        }

        [Test]
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;
            AuthenticationService authenticationService = new(unitOfWork);

            Assert.That(authenticationService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        [Test]
        public async Task TestAuthenticate_WhenUserCredentialsAreGood_ThenUserIsReturned()
        {
            Password password = new("examplepassword");
            User user = new UserBuilder()
                .WithPassword(PasswordHasher.Hash(password))
                .Build();
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(u => u.GetAsync(user.Email))
                .Returns(Task.FromResult(user));
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            AuthenticationService authenticationService = new(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(user.Email, password);

            Assert.That(authenticatedUser, Is.EqualTo(user));
        }

        [Test]
        public async Task TestAuthenticate_WhenWrongUserEmailGiven_ThenNullIsReturned()
        {
            Password password = new("examplepassword");
            User user = new UserBuilder()
                .WithPassword(PasswordHasher.Hash(password))
                .Build();
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(u => u.GetAsync(user.Email))
                .Returns(Task.FromResult(user));
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            AuthenticationService authenticationService = new(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(
                new EmailAddress("example2@example.com"),
                password);

            Assert.That(authenticatedUser, Is.Null);
        }

        [Test]
        public async Task TestAuthenticate_WhenWrongPasswordGiven_ThenNullIsReturned()
        {
            Password password = new("examplepassword");
            User user = new UserBuilder()
                .WithPassword(PasswordHasher.Hash(password))
                .Build();
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(u => u.GetAsync(user.Email))
                .Returns(Task.FromResult(user));
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepositoryMock.Object)
                .Build();
            AuthenticationService authenticationService = new(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(user.Email, new Password("wrongpassword"));

            Assert.That(authenticatedUser, Is.Null);
        }
    }
}