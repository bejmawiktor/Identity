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

        [Test]
        public void TestEquals_WhenTwoIdentitcalResourcesDtosGiven_ThenTrueIsReturned()
        {
            var leftResourceDto = new ResourceDto("MyResource", "My resource description.");
            var rightResourceDto = new ResourceDto("MyResource", "My resource description.");

            Assert.That(leftResourceDto.Equals(rightResourceDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentResourcesDtosGiven_ThenFalseIsReturned()
        {
            var leftResourceDto = new ResourceDto("MyResource", "My resource description.");
            var rightResourceDto = new ResourceDto("MyResource2", "My resource description 2.");

            Assert.That(leftResourceDto.Equals(rightResourceDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalResourcesDtosGiven_ThenSameHashCodesIsReturned()
        {
            var leftResourceDto = new ResourceDto("MyResource", "My resource description.");
            var rightResourceDto = new ResourceDto("MyResource", "My resource description.");

            Assert.That(leftResourceDto.GetHashCode(), Is.EqualTo(rightResourceDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentResourcesDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            var leftResourceDto = new ResourceDto("MyResource", "My resource description.");
            var rightResourceDto = new ResourceDto("MyResource2", "My resource description 2.");

            Assert.That(leftResourceDto.GetHashCode(), Is.Not.EqualTo(rightResourceDto.GetHashCode()));
        }
    }
}