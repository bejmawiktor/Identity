using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class EmailAddressTest
    {
        [TestCase("Abc.example.com")]
        [TestCase("a\"b(c)d,e:f;g<h>i[j\\k]l@example.com")]
        [TestCase("just\"not\"right@example.com")]
        [TestCase("this is\"not\\allowed@example.com")]
        [TestCase("this\\ still\\\"notallowed@example.com")]
        public void TestConstructor_WhenIncorrectAddressGiven_ThenArgumentExceptionIsThrown(string address)
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Incorrect email address given."),
                () => new EmailAddress(address));
        }

        [Test]
        public void TestConstructor_WhenEmptyAddressGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Email address can't be empty."),
                () => new EmailAddress(string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullAddressGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("address"),
                () => new EmailAddress(null));
        }

        [TestCase("simple@example.com")]
        [TestCase("simple@example.com.pl")]
        [TestCase("very.common@example.com")]
        [TestCase("abc@example.co.uk")]
        [TestCase("disposable.style.email.with+symbol@example.com")]
        [TestCase("___A___@example.com")]
        [TestCase("firstname-lastname@example.com")]
        [TestCase("email@123.123.123.123")]
        public void TestConstructor_WhenCorrectAddressGiven_ThenAddressIsSet(string address)
        {
            var email = new EmailAddress(address);

            Assert.That(email.ToString(), Is.EqualTo(address));
        }
    }
}