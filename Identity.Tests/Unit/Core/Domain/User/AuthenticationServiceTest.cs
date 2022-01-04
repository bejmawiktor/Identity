using Identity.Core.Domain;
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
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            AuthenticationService authenticationService = new(unitOfWork);

            Assert.That(authenticationService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        private IUnitOfWork GetUnitOfWork(IUsersRepository usersRepository = null)
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(x => x.UsersRepository)
                .Returns(usersRepository ?? new Mock<IUsersRepository>().Object);
            IUnitOfWork unitOfWork = unitOfWorkMock.Object;

            return unitOfWork;
        }

        [Test]
        public async Task TestAuthenticate_WhenUserCredentialsAreGood_ThenUserIsReturned()
        {
            Password password = new("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            EmailAddress emailAddress = new("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(emailAddress)).Returns(Task.FromResult(user));
            IUnitOfWork unitOfWork = this.GetUnitOfWork(usersRepositoryMock.Object);
            AuthenticationService authenticationService = new(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(emailAddress, password);

            Assert.That(authenticatedUser, Is.EqualTo(user));
        }

        [Test]
        public async Task TestAuthenticate_WhenWrongUserEmailGiven_ThenNullIsReturned()
        {
            Password password = new("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            EmailAddress emailAddress = new("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(emailAddress)).Returns(Task.FromResult(user));
            IUnitOfWork unitOfWork = this.GetUnitOfWork(usersRepositoryMock.Object);
            AuthenticationService authenticationService = new(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(new EmailAddress("example2@example.com"), password);

            Assert.That(authenticatedUser, Is.Null);
        }

        [Test]
        public async Task TestAuthenticate_WhenWrongPasswordGiven_ThenNullIsReturned()
        {
            Password password = new("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            EmailAddress emailAddress = new("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(emailAddress)).Returns(Task.FromResult(user));
            IUnitOfWork unitOfWork = this.GetUnitOfWork(usersRepositoryMock.Object);
            AuthenticationService authenticationService = new(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(emailAddress, new Password("wrongpassword"));

            Assert.That(authenticatedUser, Is.Null);
        }
    }
}