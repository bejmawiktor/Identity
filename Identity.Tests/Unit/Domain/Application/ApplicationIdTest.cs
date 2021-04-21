using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationIdTest
    {
        [Test]
        public void TestConstructing_WhenEmptyGuidGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Guid can't be empty."),
              () => new ApplicationId(Guid.Empty));
        }

        [Test]
        public void TestToGuid_WhenConvertingToGuid_ThenGuidIsReturned()
        {
            var guid = Guid.NewGuid();
            var applicationId = new ApplicationId(guid);

            Assert.That(applicationId.ToGuid(), Is.EqualTo(guid));
        }

        [Test]
        public void TestGenerate_WhenGeneretingRoleId_ThenNewRoleIdIsReturned()
        {
            var applicationId = ApplicationId.Generate();

            Assert.That(applicationId, Is.Not.Null);
        }
    }
}