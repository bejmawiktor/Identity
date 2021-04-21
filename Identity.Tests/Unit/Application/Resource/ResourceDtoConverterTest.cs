using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class ResourceDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenResourceGiven_ThenResourceDtoIsReturned()
        {
            var resourceId = new ResourceId("MyResource");
            var resource = new Resource(
                id: resourceId,
                description: "My resource description");
            var resourceDtoConverter = new ResourceDtoConverter();

            ResourceDto resourceDto = resourceDtoConverter.ToDto(resource);

            Assert.That(resourceDto.Id, Is.EqualTo(resourceId.ToString()));
            Assert.That(resourceDto.Description, Is.EqualTo("My resource description"));
        }

        [Test]
        public void TestToDto_WhenNullResourceGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourceDtoConverter = new ResourceDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("resource"),
                () => resourceDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenResourceIdGiven_ThenDtoIdentifierIsReturned()
        {
            var resourceId = new ResourceId("MyResource");
            var resourceDtoConverter = new ResourceDtoConverter();

            string resourceDtoIdentifier = resourceDtoConverter.ToDtoIdentifier(resourceId);

            Assert.That(resourceDtoIdentifier, Is.EqualTo(resourceId.ToString()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullResourceIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var resourceDtoConverter = new ResourceDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("resourceId"),
                () => resourceDtoConverter.ToDtoIdentifier(null));
        }
    }
}