using Identity.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class CreateResourceCommandHandlerTest
    {
        [Test]
        public void TestConstructor_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var resourcesRepositoryMock = new Mock<IResourcesRepository>();
            IResourcesRepository resourcesRepository = resourcesRepositoryMock.Object;
            var createResourceCommandHandler = new CreateResourceCommandHandler(resourcesRepository);

            Assert.That(createResourceCommandHandler.ResourcesRepository, Is.EqualTo(resourcesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("resourcesRepository"),
               () => new CreateResourceCommandHandler(null));
        }
    }
}