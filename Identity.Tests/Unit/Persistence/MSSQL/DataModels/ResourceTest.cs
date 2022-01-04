using Identity.Core.Application;
using Identity.Persistence.MSSQL.DataModels;
using NUnit.Framework;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    public class ResourceTest
    {
        [Test]
        public void TestConstructor_WhenDtoGiven_ThenMembersAreSet()
        {
            Resource resource = new(
                new ResourceDto("MyResource", "My resource description."));

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo("MyResource"));
                Assert.That(resource.Description, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestSetFields_WhenDtoGiven_ThenMembersAreSet()
        {
            Resource resource = new();

            resource.SetFields(new ResourceDto("MyResource", "My resource description."));

            Assert.Multiple(() =>
            {
                Assert.That(resource.Id, Is.EqualTo("MyResource"));
                Assert.That(resource.Description, Is.EqualTo("My resource description."));
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenResourceDtoIsReturned()
        {
            Resource resource = new()
            {
                Id = "MyResource",
                Description = "My resource description."
            };

            ResourceDto resourceDto = resource.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(resourceDto.Id, Is.EqualTo("MyResource"));
                Assert.That(resourceDto.Description, Is.EqualTo("My resource description."));
            });
        }
    }
}