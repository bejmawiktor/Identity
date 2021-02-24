using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class ResourceCreatedEventTest
    {
        [Test]
        public void TestConstruction_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            var resourceCreatedEvent = new ResourceCreatedEvent(
                resourceId: new ResourceId("TestResource"),
                resourceDescription: "Test resource description");

            Assert.That(resourceCreatedEvent.ResourceId, Is.EqualTo(new ResourceId("TestResource")));
        }

        [Test]
        public void TestConstruction_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            var resourceCreatedEvent = new ResourceCreatedEvent(
                resourceId: new ResourceId("TestResource"),
                resourceDescription: "Test resource description");

            Assert.That(resourceCreatedEvent.ResourceDescription, Is.EqualTo("Test resource description"));
        }
    }
}