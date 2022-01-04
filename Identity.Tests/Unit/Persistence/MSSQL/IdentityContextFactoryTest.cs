using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class IdentityContextFactoryTest
    {
        [Test]
        public void TestCreateDbContext_WhenNullArgsGiven_ThenArgumentNullExceptionIsThrown()
        {
            IdentityContextFactory identityContextFactory = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("args"),
                () => identityContextFactory.CreateDbContext(null));
        }

        [Test]
        public void TestCreateDbContext_WhenEmptyArgsGiven_ThenArgumentExceptionIsThrown()
        {
            IdentityContextFactory identityContextFactory = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Args must contain connectionString."),
                () => identityContextFactory.CreateDbContext(Array.Empty<string>()));
        }

        [Test]
        public void TestCreateDbContext_WhenConnectionStringsGivenInArgs_ThenIdentityContextIsReturned()
        {
            IdentityContextFactory identityContextFactory = new();

            IdentityContext identityContext = identityContextFactory.CreateDbContext(new string[]
            {
                @"Server=server;Database=db;uid=uidsr;Password=password;"
            });

            Assert.That(identityContext, Is.Not.Null);
        }
    }
}