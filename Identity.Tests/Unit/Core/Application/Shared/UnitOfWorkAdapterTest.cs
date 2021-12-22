using DDD.Domain.Persistence;
using Identity.Core.Application;
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
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: new Mock<IApplicationsRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.ApplicationsRepository, Is.Not.Null);
        }

        private IUnitOfWork GetUnitOfWork(
            IApplicationsRepository applicationsRepository = null,
            IAuthorizationCodesRepository authorizationCodesRepository = null,
            IPermissionsRepository permissionsRepository = null,
            IResourcesRepository resourcesRepository = null,
            IRolesRepository rolesRepository = null,
            IRefreshTokensRepository refreshTokensRepository = null,
            IUsersRepository usersRepository = null)
        {
            Mock<IUnitOfWork> unitOfWorkMock = this.GetUnitOfWorkMock(
                applicationsRepository,
                authorizationCodesRepository,
                permissionsRepository,
                resourcesRepository,
                rolesRepository,
                refreshTokensRepository,
                usersRepository);

            return unitOfWorkMock.Object;
        }

        private Mock<IUnitOfWork> GetUnitOfWorkMock(
            IApplicationsRepository applicationsRepository = null,
            IAuthorizationCodesRepository authorizationCodesRepository = null,
            IPermissionsRepository permissionsRepository = null,
            IResourcesRepository resourcesRepository = null,
            IRolesRepository rolesRepository = null,
            IRefreshTokensRepository refreshTokensRepository = null,
            IUsersRepository usersRepository = null)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ApplicationsRepository)
                .Returns(applicationsRepository ?? new Mock<IApplicationsRepository>().Object);
            unitOfWorkMock.Setup(x => x.AuthorizationCodesRepository)
                .Returns(authorizationCodesRepository ?? new Mock<IAuthorizationCodesRepository>().Object);
            unitOfWorkMock.Setup(x => x.PermissionsRepository)
                .Returns(permissionsRepository ?? new Mock<IPermissionsRepository>().Object);
            unitOfWorkMock.Setup(x => x.ResourcesRepository)
                .Returns(resourcesRepository ?? new Mock<IResourcesRepository>().Object);
            unitOfWorkMock.Setup(x => x.RolesRepository)
                .Returns(rolesRepository ?? new Mock<IRolesRepository>().Object);
            unitOfWorkMock.Setup(x => x.RefreshTokensRepository)
                .Returns(refreshTokensRepository ?? new Mock<IRefreshTokensRepository>().Object);
            unitOfWorkMock.Setup(x => x.UsersRepository)
                .Returns(usersRepository ?? new Mock<IUsersRepository>().Object);

            return unitOfWorkMock;
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenAuthorizationCodesRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                authorizationCodesRepository: new Mock<IAuthorizationCodesRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.AuthorizationCodesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenPermissionsRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                permissionsRepository: new Mock<IPermissionsRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.PermissionsRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenResourcesRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                resourcesRepository: new Mock<IResourcesRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.ResourcesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenRolesRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                rolesRepository: new Mock<IRolesRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.RolesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenRefreshTokensRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                refreshTokensRepository: new Mock<IRefreshTokensRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.RefreshTokensRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenApplicationUnitOfWorkGiven_ThenUsersRepositoryIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: new Mock<IUsersRepository>().Object);

            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWork);

            Assert.That(unitOfWorkAdapter.UsersRepository, Is.Not.Null);
        }

        [Test]
        public void TestBeginScope_WhenBeginning_ThenApplicationTransactionScopeIsReturned()
        {
            var transactionScope = new Mock<ITransactionScope>().Object;
            Mock<IUnitOfWork> unitOfWorkMock = this.GetUnitOfWorkMock();
            unitOfWorkMock.Setup(x => x.BeginScope())
                .Returns(transactionScope);
            var unitOfWorkAdapter = new UnitOfWorkAdapter(unitOfWorkMock.Object);

            ITransactionScope resultedTransactionScope = unitOfWorkAdapter.BeginScope();

            Assert.That(resultedTransactionScope, Is.EqualTo(transactionScope));
        }
    }
}