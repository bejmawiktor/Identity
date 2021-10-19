using Identity.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    public class AuthorizationCodesRepositoryAdapterTest
    {
        [Test]
        public void TestConstructing_WhenNullAuthorizationCodesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("authorizationCodesRepository"),
               () => new AuthorizationCodesRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructing_WhenAuthorizationCodesRepositoryGiven_ThenAuthorizationCodesRepositoryIsSet()
        {
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationCodesRepositoryAdapter = new AuthorizationCodesRepositoryAdapter(authorizationCodesRepository);

            Assert.That(authorizationCodesRepositoryAdapter.AuthorizationCodesRepository, Is.EqualTo(authorizationCodesRepository));
        }
    }
}
