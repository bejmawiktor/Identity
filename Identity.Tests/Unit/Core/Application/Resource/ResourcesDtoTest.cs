using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class ResourcesDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            ResourceDto resourceDto = new ResourceDtoBuilder()
                .WithId("MyResource2")
                .Build();

            Assert.That(resourceDto.Id, Is.EqualTo("MyResource2"));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            ResourceDto resourceDto = new ResourceDtoBuilder()
                .WithDescription("My resource description 2.")
                .Build();

            Assert.That(resourceDto.Description, Is.EqualTo("My resource description 2."));
        }

        [Test]
        public void TestToResource_WhenConvertingToResource_ThenResourceIsReturned()
        {
            ResourceDto resourceDto = ResourceDtoBuilder.DefaultResourceDto;

            Resource resource = resourceDto.ToResource();

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo(new ResourceId(resourceDto.Id)));
                Assert.That(resource.Description, Is.EqualTo(resourceDto.Description));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalResourcesDtosGiven_ThenTrueIsReturned()
        {
            ResourceDto leftResourceDto = ResourceDtoBuilder.DefaultResourceDto;
            ResourceDto rightResourceDto = ResourceDtoBuilder.DefaultResourceDto;

            Assert.That(leftResourceDto.Equals(rightResourceDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentResourcesDtosGiven_ThenFalseIsReturned()
        {
            ResourceDto leftResourceDto = ResourceDtoBuilder.DefaultResourceDto;
            ResourceDto rightResourceDto = new ResourceDtoBuilder()
                .WithId("MyResource2")
                .WithDescription("My resource description 2.")
                .Build();

            Assert.That(leftResourceDto.Equals(rightResourceDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalResourcesDtosGiven_ThenSameHashCodesAreReturned()
        {
            ResourceDto leftResourceDto = ResourceDtoBuilder.DefaultResourceDto;
            ResourceDto rightResourceDto = ResourceDtoBuilder.DefaultResourceDto;

            Assert.That(leftResourceDto.GetHashCode(), Is.EqualTo(rightResourceDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentResourcesDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            ResourceDto leftResourceDto = ResourceDtoBuilder.DefaultResourceDto;
            ResourceDto rightResourceDto = new ResourceDtoBuilder()
                .WithId("MyResource2")
                .WithDescription("My resource description 2.")
                .Build();

            Assert.That(leftResourceDto.GetHashCode(), Is.Not.EqualTo(rightResourceDto.GetHashCode()));
        }
    }
}