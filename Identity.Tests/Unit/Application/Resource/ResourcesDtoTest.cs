using Identity.Application;
using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class ResourcesDtoTest
    {
        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            var resourceDto = new ResourceDto("MyResource", "My resource description.");

            Assert.That(resourceDto.Id, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructing_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var resourceDto = new ResourceDto("MyResource", "My resource description.");

            Assert.That(resourceDto.Description, Is.EqualTo("My resource description."));
        }

        [Test]
        public void TestToResource_WhenConvertingToResource_ThenResourceIsReturned()
        {
            var resourceDto = new ResourceDto("MyResource", "My resource description.");

            Resource resource = resourceDto.ToResource();

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resource.Description, Is.EqualTo("My resource description."));
            });
        }
    }
}