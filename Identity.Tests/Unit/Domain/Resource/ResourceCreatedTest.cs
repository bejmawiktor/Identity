using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class ResourceCreatedTest
    {
        [Test]
        public void TestConstructor_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            var resourceCreated = new ResourceCreated(
                resourceId: new ResourceId("TestResource"),
                resourceDescription: "Test resource description");

            Assert.That(resourceCreated.ResourceId, Is.EqualTo(new ResourceId("TestResource")));
        }

        [Test]
        public void TestConstructor_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            var resourceCreated = new ResourceCreated(
                resourceId: new ResourceId("TestResource"),
                resourceDescription: "Test resource description");

            Assert.That(resourceCreated.ResourceDescription, Is.EqualTo("Test resource description"));
        }
    }
}