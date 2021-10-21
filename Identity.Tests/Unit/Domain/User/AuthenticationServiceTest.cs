﻿using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        [Test]
        public void TestConstructing_WhenNullUserRepositoryGiven_ThenArgumentNullIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new AuthenticationService(null));
        }

        [Test]
        public void TestAuthenticate_WhenUserCredentialsAreGood_ThenUserIsReturned()
        {
            var password = new Password("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(
                email: emailAddress, 
                password: hashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(u => u.Get(emailAddress))
                .Returns(user);
            var authenticationService = new AuthenticationService(usersRepositoryMock.Object);

            User authenticatedUser = authenticationService.Authenticate(emailAddress, password);

            Assert.That(authenticatedUser, Is.EqualTo(user));
        }

        [Test]
        public void TestAuthenticate_WhenWrongUserEmailGiven_ThenNullIsReturned()
        {
            var password = new Password("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(u => u.Get(emailAddress))
                .Returns(user);
            var authenticationService = new AuthenticationService(usersRepositoryMock.Object);

            User authenticatedUser = authenticationService.Authenticate(new EmailAddress("example2@example.com"), password);

            Assert.That(authenticatedUser, Is.Null);
        }

        [Test]
        public void TestAuthenticate_WhenWrongPasswordGiven_ThenNullIsReturned()
        {
            var password = new Password("examplepassword");
            HashedPassword hashedPassword = HashedPassword.Hash(password);
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(
                email: emailAddress,
                password: hashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(u => u.Get(emailAddress))
                .Returns(user);
            var authenticationService = new AuthenticationService(usersRepositoryMock.Object);

            User authenticatedUser = authenticationService.Authenticate(emailAddress, new Password("wrongpassword"));

            Assert.That(authenticatedUser, Is.Null);
        }
    }
}
