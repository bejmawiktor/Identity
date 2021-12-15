﻿using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Domain
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
            var unitOfWork = this.GetUnitOfWork();
            var authenticationService = new AuthenticationService(unitOfWork);

            Assert.That(authenticationService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        private IUnitOfWork GetUnitOfWork(IUsersRepository usersRepository = null)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.UsersRepository)
                .Returns(usersRepository ?? new Mock<IUsersRepository>().Object);
            var unitOfWork = unitOfWorkMock.Object;

            return unitOfWork;
        }

        [Test]
        public async Task TestAuthenticate_WhenUserCredentialsAreGood_ThenUserIsReturned()
        {
            var password = new Password("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(emailAddress)).Returns(Task.FromResult(user));
            var unitOfWork = this.GetUnitOfWork(usersRepositoryMock.Object);
            var authenticationService = new AuthenticationService(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(emailAddress, password);

            Assert.That(authenticatedUser, Is.EqualTo(user));
        }

        [Test]
        public async Task TestAuthenticate_WhenWrongUserEmailGiven_ThenNullIsReturned()
        {
            var password = new Password("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(emailAddress)).Returns(Task.FromResult(user));
            var unitOfWork = this.GetUnitOfWork(usersRepositoryMock.Object);
            var authenticationService = new AuthenticationService(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(new EmailAddress("example2@example.com"), password);

            Assert.That(authenticatedUser, Is.Null);
        }

        [Test]
        public async Task TestAuthenticate_WhenWrongPasswordGiven_ThenNullIsReturned()
        {
            var password = new Password("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(emailAddress)).Returns(Task.FromResult(user));
            var unitOfWork = this.GetUnitOfWork(usersRepositoryMock.Object);
            var authenticationService = new AuthenticationService(unitOfWork);

            User authenticatedUser = await authenticationService.Authenticate(emailAddress, new Password("wrongpassword"));

            Assert.That(authenticatedUser, Is.Null);
        }
    }
}