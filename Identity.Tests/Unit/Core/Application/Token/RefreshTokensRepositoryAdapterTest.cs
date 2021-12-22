using Identity.Core.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    using IRefreshTokensRepository = Identity.Core.Application.IRefreshTokensRepository;

    [TestFixture]
    public class RefreshTokensRepositoryAdapterTest
    {
        [Test]
        public void TestConstructor_WhenNullRolesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("refreshTokensRepository"),
               () => new RefreshTokensRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructor_WhenRolesRepositoryGiven_ThenRolesRepositoryIsSet()
        {
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            var refreshTokensRepositoryAdapter = new RefreshTokensRepositoryAdapter(refreshTokensRepository);

            Assert.That(refreshTokensRepositoryAdapter.RefreshTokensRepository, Is.EqualTo(refreshTokensRepository));
        }
    }
}
