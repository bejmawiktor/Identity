using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class IdentityContextTest
    {
        [Test]
        public void TestConstructing_WhenNullConnectionStringGiven_ThenArgumentNullExceptionIsThrown()
        {
            var identityContext = new IdentityContext("TestConnectionString");

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("connectionString"),
                () => new IdentityContext((string)null));
        }
    }
}