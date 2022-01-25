using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Core.Application
{
    using IUsersRepository = Identity.Core.Application.IUsersRepository;

    [TestFixture]
    public class UsersRepositoryAdapterTest
    {
        private readonly HashedPassword HashedPassword = HashedPassword.Hash(new Password("examplepassword"));

        [Test]
        public void TestConstructor_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new UsersRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructor_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            Mock<IUsersRepository> usersRepositoryMock = new();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            UsersRepositoryAdapter usersRepositoryAdapter = new(usersRepository);

            Assert.That(usersRepositoryAdapter.UsersRepository, Is.EqualTo(usersRepository));
        }

        [Test]
        public async Task TestGetAsync_WhenUserWithEmailRegistered_ThenUserIsReturned()
        {
            User user = UserBuilder.DefaultUser;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(u => u.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new UserDtoConverter().ToDto(user)));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            UsersRepositoryAdapter usersRepositoryAdapter = new(usersRepository);

            User result = await usersRepositoryAdapter.GetAsync(user.Email);

            Assert.That(result, Is.EqualTo(user));
        }
    }
}