﻿using Identity.Application;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Application
{
    using IUsersRepository = Identity.Application.IUsersRepository;

    [TestFixture]
    public class UsersRepositoryAdapterTest
    {
        private readonly HashedPassword HashedPassword = HashedPassword.Hash(new Password("examplepassword"));

        [Test]
        public void TestConstructing_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new UsersRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructing_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            var usersRepositoryAdapter = new UsersRepositoryAdapter(usersRepository);

            Assert.That(usersRepositoryAdapter.UsersRepository, Is.EqualTo(usersRepository));
        }

        [Test]
        public void TestGet_WhenUserWithEmailRegistered_ThenUserIsReturned()
        {
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(new EmailAddress("example@example.com"), this.HashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(u => u.Get(It.IsAny<string>()))
                .Returns(new UserDtoConverter().ToDto(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            var usersRepositoryAdapter = new UsersRepositoryAdapter(usersRepository);

            User result = usersRepositoryAdapter.Get(emailAddress);

            Assert.That(result, Is.EqualTo(user));
        }

        [Test]
        public async Task TestGetAsync_WhenUserWithEmailRegistered_ThenUserIsReturned()
        {
            var emailAddress = new EmailAddress("example@example.com");
            User user = User.Create(new EmailAddress("example@example.com"), this.HashedPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(u => u.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new UserDtoConverter().ToDto(user)));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            var usersRepositoryAdapter = new UsersRepositoryAdapter(usersRepository);

            User result = await usersRepositoryAdapter.GetAsync(emailAddress);

            Assert.That(result, Is.EqualTo(user));
        }
    }
}