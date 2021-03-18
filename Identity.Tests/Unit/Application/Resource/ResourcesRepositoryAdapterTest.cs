using Identity.Application;
using Moq;
using NUnit.Framework;
using System;
using IResourcesRepository = Identity.Application.IResourcesRepository;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class ResourcesRepositoryAdapterTest
    {
        [Test]
        public void TestConstructing_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("resourcesRepository"),
               () => new ResourcesRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructing_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            var resourcesRepositoryAdapter = new ResourcesRepositoryAdapter(resourcesRepository);

            Assert.That(resourcesRepositoryAdapter.ResourcesRepository, Is.EqualTo(resourcesRepository));
        }
    }
}