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
            var resourceId = new ResourceId("TestResource");

            ResourceCreated resourceCreated = this.GetResourceCreated(
                resourceId: new ResourceId("TestResource"));

            Assert.That(resourceCreated.ResourceId, Is.EqualTo(resourceId.ToString()));
        }

        private ResourceCreated GetResourceCreated(
            ResourceId resourceId = null, 
            string resourceDescription = null)
        {
            return new ResourceCreated(
                resourceId: resourceId ?? new ResourceId("TestResource"),
                resourceDescription: resourceDescription ?? "Test resource description");
        }

        [Test]
        public void TestConstructor_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            ResourceCreated resourceCreated = this.GetResourceCreated(
                resourceDescription: "Test resource description");

            Assert.That(resourceCreated.ResourceDescription, Is.EqualTo("Test resource description"));
        }
    }
}