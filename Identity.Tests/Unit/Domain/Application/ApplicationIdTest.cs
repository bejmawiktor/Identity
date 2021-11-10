using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class ApplicationIdTest
    {
        [Test]
        public void TestConstructor_WhenEmptyGuidGiven_ThenArgumentExceptionIsThrown()
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
            Guid guid = Guid.NewGuid();
            ApplicationId applicationId = new ApplicationId(guid);

            Assert.That(applicationId.ToGuid(), Is.EqualTo(guid));
        }

        [Test]
        public void TestGenerate_WhenGeneratingApplicationId_ThenNewApplicationIdIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();

            Assert.That(applicationId, Is.Not.Null);
        }

        [Test]
        public void TestToString_WhenConvertingToString_ThenGuidStringIsReturned()
        {
            Guid guid = Guid.NewGuid();
            ApplicationId applicationId = new ApplicationId(guid);

            string applicationIdString = applicationId.ToString();

            Assert.That(applicationIdString, Is.EqualTo(guid.ToString()));
        }
    }
}