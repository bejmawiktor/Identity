using Identity.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class UsersRepositoryAdapterTest
    {
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
    }
}