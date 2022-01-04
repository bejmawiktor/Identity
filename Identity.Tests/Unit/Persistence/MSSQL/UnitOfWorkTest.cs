using DDD.Domain.Persistence;
using Identity.Persistence.MSSQL;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class UnitOfWorkTest
    {
        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenApplicationsRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.ApplicationsRepository, Is.Not.Null);
        }

        private UnitOfWork GetUnitOfWork()
        {
            var options = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
            IdentityContext identityContext = new(options);
            UnitOfWork unitOfWork = new(identityContext);
            return unitOfWork;
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenAuthorizationCodesRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.AuthorizationCodesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenPermissionsRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.PermissionsRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenRefreshTokensRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.RefreshTokensRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenResourcesRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.ResourcesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenRolesRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.RolesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenUsersRepositoryIsSet()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            Assert.That(unitOfWork.UsersRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenNullIdentityContextGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("identityContext"),
                () => new UnitOfWork(null));
        }

        [Test]
        public void TestBeginScope_WhenBegins_ThenTransactionScopeIsReturned()
        {
            UnitOfWork unitOfWork = this.GetUnitOfWork();

            ITransactionScope transactionScope = unitOfWork.BeginScope();

            Assert.That(transactionScope, Is.Not.Null);
        }
    }
}