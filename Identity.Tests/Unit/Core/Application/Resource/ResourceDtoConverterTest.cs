using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class ResourceDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenResourceGiven_ThenResourceDtoIsReturned()
        {
            ResourceId resourceId = new("MyResource");
            Resource resource = new(
                id: resourceId,
                description: "My resource description");
            ResourceDtoConverter resourceDtoConverter = new();

            ResourceDto resourceDto = resourceDtoConverter.ToDto(resource);

            Assert.That(resourceDto.Id, Is.EqualTo(resourceId.ToString()));
            Assert.That(resourceDto.Description, Is.EqualTo("My resource description"));
        }

        [Test]
        public void TestToDto_WhenNullResourceGiven_ThenArgumentNullExceptionIsThrown()
        {
            ResourceDtoConverter resourceDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("resource"),
                () => resourceDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenResourceIdGiven_ThenDtoIdentifierIsReturned()
        {
            ResourceId resourceId = new("MyResource");
            ResourceDtoConverter resourceDtoConverter = new();

            string resourceDtoIdentifier = resourceDtoConverter.ToDtoIdentifier(resourceId);

            Assert.That(resourceDtoIdentifier, Is.EqualTo(resourceId.ToString()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullResourceIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            ResourceDtoConverter resourceDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("resourceId"),
                () => resourceDtoConverter.ToDtoIdentifier(null));
        }
    }
}