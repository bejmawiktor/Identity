using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class IdentityContextTest
    {
        [Test]
        public void TestConstructor_WhenNullConnectionStringGiven_ThenArgumentNullExceptionIsThrown()
        {
            IdentityContext identityContext = new("TestConnectionString");

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("connectionString"),
                () => new IdentityContext((string)null));
        }
    }
}