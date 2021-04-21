using Identity.Persistence.MSSQL;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public abstract class DatabaseTestBase
    {
        protected IdentityContext IdentityContext { get; private set; }

        [SetUp]
        public void SetupTransaction()
        {
            var options = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
            this.IdentityContext = new IdentityContext(options);
        }

        [TearDown]
        public void RollbackTransaction()
        {
            this.IdentityContext.Database.EnsureDeleted();
            this.IdentityContext.Dispose();
        }
    }
}