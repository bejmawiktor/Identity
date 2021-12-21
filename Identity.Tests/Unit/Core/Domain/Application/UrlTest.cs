using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using Url = Identity.Core.Domain.Url;

    [TestFixture]
    public class UrlTest
    {
        [Test]
        public void TestConstructor_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new Url(null));
        }

        [Test]
        public void TestConstructor_WhenEmptyValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Url can't be empty."),
                () => new Url(string.Empty));
        }

        [TestCase("http://.www.foo.bar./")]
        [TestCase("ftps://foo.bar/")]
        [TestCase("ftp://foo.bar/")]
        [TestCase(":// should fail")]
        [TestCase("http:// shouldfail.com")]
        [TestCase("//")]
        [TestCase("http://##/")]
        [TestCase("asfasfsgaf gafg ")]
        [TestCase(" ")]
        [TestCase("www.foo.bar")]
        public void TestConstructor_WhenInvalidUrlGiven_ThenArgumentExceptionIsThrown(string value)
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Invalid url given."),
                () => new Url(value));
        }

        [TestCase("http://www.foo.bar/")]
        [TestCase("http://foo.com/blah_blah/")]
        [TestCase("https://www.example.com/foo/?bar=baz&inga=42&quux")]
        [TestCase("https://www.google.pl")]
        public void TestToString_WhenValidUrlGiven_ThenUrlValueIsReturned(string value)
        {
            var url = new Url(value);

            Assert.That(url.ToString(), Is.EqualTo(value));
        }
    }
}