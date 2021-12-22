using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class ResourcesDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            ResourceDto resourceDto = this.GetResourceDto("MyResource");

            Assert.That(resourceDto.Id, Is.EqualTo("MyResource"));
        }

        private ResourceDto GetResourceDto(
            string id = null,
            string description = null)
        {
            return new ResourceDto(
                id ?? "MyResource",
                description ?? "My resource description.");
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            ResourceDto resourceDto = this.GetResourceDto(description: "My resource description.");

            Assert.That(resourceDto.Description, Is.EqualTo("My resource description."));
        }

        [Test]
        public void TestToResource_WhenConvertingToResource_ThenResourceIsReturned()
        {
            ResourceDto resourceDto = this.GetResourceDto("MyResource", "My resource description.");

            Resource resource = resourceDto.ToResource();

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo(new ResourceId("MyResource")));
                Assert.That(resource.Description, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalResourcesDtosGiven_ThenTrueIsReturned()
        {
            ResourceDto leftResourceDto = this.GetResourceDto();
            ResourceDto rightResourceDto = this.GetResourceDto();

            Assert.That(leftResourceDto.Equals(rightResourceDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentResourcesDtosGiven_ThenFalseIsReturned()
        {
            ResourceDto leftResourceDto = this.GetResourceDto("MyResource", "My resource description.");
            ResourceDto rightResourceDto = this.GetResourceDto("MyResource2", "My resource description 2.");

            Assert.That(leftResourceDto.Equals(rightResourceDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalResourcesDtosGiven_ThenSameHashCodesAreReturned()
        {
            ResourceDto leftResourceDto = this.GetResourceDto();
            ResourceDto rightResourceDto = this.GetResourceDto();

            Assert.That(leftResourceDto.GetHashCode(), Is.EqualTo(rightResourceDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentResourcesDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            ResourceDto leftResourceDto = this.GetResourceDto("MyResource", "My resource description.");
            ResourceDto rightResourceDto = this.GetResourceDto("MyResource2", "My resource description 2.");

            Assert.That(leftResourceDto.GetHashCode(), Is.Not.EqualTo(rightResourceDto.GetHashCode()));
        }
    }
}