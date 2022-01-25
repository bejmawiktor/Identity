using DDD.Domain.Persistence;
using Identity.Persistence.MSSQL;
using Identity.Tests.Unit.Persistence.MSSQL.Builders;
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
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            Assert.That(unitOfWork.ApplicationsRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenAuthorizationCodesRepositoryIsSet()
        {
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            Assert.That(unitOfWork.AuthorizationCodesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenPermissionsRepositoryIsSet()
        {
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            Assert.That(unitOfWork.PermissionsRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenRefreshTokensRepositoryIsSet()
        {
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            Assert.That(unitOfWork.RefreshTokensRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenResourcesRepositoryIsSet()
        {
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            Assert.That(unitOfWork.ResourcesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenRolesRepositoryIsSet()
        {
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            Assert.That(unitOfWork.RolesRepository, Is.Not.Null);
        }

        [Test]
        public void TestConstructor_WhenIdentityContextGiven_ThenUsersRepositoryIsSet()
        {
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

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
            UnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            ITransactionScope transactionScope = unitOfWork.BeginScope();

            Assert.That(transactionScope, Is.Not.Null);
        }
    }
}