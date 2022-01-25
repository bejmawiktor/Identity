using Identity.Persistence.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace Identity.Tests.Unit.Persistence.MSSQL.Builders
{
    internal class UnitOfWorkBuilder
    {
        public static UnitOfWork DefaultUnitOfWork => new UnitOfWorkBuilder().Build();

        public IdentityContext IdentityContext { get; private set; }
            = new (new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options);

        public UnitOfWorkBuilder WithIdentityContext(IdentityContext identityContext)
        {
            this.IdentityContext = identityContext;

            return this;
        }

        public UnitOfWork Build()
            => new UnitOfWork(this.IdentityContext);
    }
}