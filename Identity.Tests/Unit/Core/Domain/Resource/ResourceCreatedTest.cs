using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class ResourceCreatedTest
    {
        [Test]
        public void TestConstructor_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            ResourceId resourceId = new("TestResource2");

            ResourceCreated resourceCreated = new ResourceCreatedBuilder()
                .WithResourceId(resourceId)
                .Build();

            Assert.That(resourceCreated.ResourceId, Is.EqualTo(resourceId.ToString()));
        }

        [Test]
        public void TestConstructor_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            ResourceCreated resourceCreated = new ResourceCreatedBuilder()
                .WithResourceDescription("Test resource description 2.")
                .Build();
        
            Assert.That(resourceCreated.ResourceDescription, Is.EqualTo("Test resource description 2."));
        }
    }
}