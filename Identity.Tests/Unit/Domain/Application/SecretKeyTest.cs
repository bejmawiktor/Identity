using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class SecretKeyTest
    {
        [Test]
        public void TestConstructor_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new SecretKey(null));
        }

        [Test]
        public void TestConstructor_WhenEmptyValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Secret key can't be empty."),
                () => new SecretKey(string.Empty));
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotNullSecretKeyIsReturned()
        {
            SecretKey secretKey = SecretKey.Generate();

            Assert.That(secretKey.ToString(), Is.Not.Null);
        }

        [Test]
        public void TestGenerate_WhenGenerated_ThenNotEmptySecretKeyIsReturned()
        {
            SecretKey secretKey = SecretKey.Generate();

            Assert.That(secretKey.ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestGenerate_WhenMultipleGenerated_ThenKeysHaveDifferentValues()
        {
            SecretKey firstSecretKey = SecretKey.Generate();
            SecretKey secondSecretKey = SecretKey.Generate();

            Assert.That(firstSecretKey.ToString(), Is.Not.EqualTo(secondSecretKey.ToString()));
        }
    }
}