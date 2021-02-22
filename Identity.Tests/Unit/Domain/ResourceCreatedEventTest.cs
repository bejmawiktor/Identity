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
                description: "Test resource description");

            Assert.That(resourceCreatedEvent.ResourceId, Is.EqualTo(new ResourceId("TestResource")));
        }

        [Test]
        public void TestConstruction_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var resourceCreatedEvent = new ResourceCreatedEvent(
                resourceId: new ResourceId("TestResource"),
                description: "Test resource description");

            Assert.That(resourceCreatedEvent.Description, Is.EqualTo("Test resource description"));
        }
    }
}