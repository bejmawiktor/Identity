using DDD.Domain.Persistence;
using Identity.Core.Application;
using Identity.Tests.Unit.Core.Application.Builders;
using Moq;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Application
{
    using IUnitOfWork = Identity.Core.Application.IUnitOfWork;

    [TestFixture]
    public class UnitOfWorkAdapterTest
    {
        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenApplicationsRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.ApplicationsRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenAuthorizationCodesRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.AuthorizationCodesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenPermissionsRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.PermissionsRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenResourcesRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.ResourcesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenRolesRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.RolesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenRefreshTokensRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.RefreshTokensRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenUsersRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWork);

            Assert.That(unitOfWorkAdapter.UsersRepository, Is.Not.Null);
        }

        [Test]
        public void TestBeginScope_WhenBeginning_ThenApplicationTransactionScopeIsReturned()
        {
            ITransactionScope transactionScope = new Mock<ITransactionScope>().Object;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(x => x.BeginScope())
                .Returns(transactionScope);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .Build();
            UnitOfWorkAdapter unitOfWorkAdapter = new(unitOfWorkMock.Object);

            ITransactionScope resultedTransactionScope = unitOfWorkAdapter.BeginScope();

            Assert.That(resultedTransactionScope, Is.EqualTo(transactionScope));
        }
    }
}