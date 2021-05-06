using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PasswordTest
    {
        [Test]
        public void TestConstructing_WhenNullValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"),
                () => new Password(null));
        }

        [Test]
        public void TestConstructing_WhenEmptyValueGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Password can't be empty."),
                () => new Password(string.Empty));
        }

        [TestCase("1")]
        [TestCase("1a")]
        [TestCase("1ab")]
        [TestCase("1abc")]
        [TestCase("1abcd")]
        [TestCase("1abcde")]
        public void TestConstructing_WhenValueIsShorterThan7Characters_ThenArgumentExceptionIsThrown(string value)
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Password must be longer than 6 characters."),
                () => new Password(value));
        }

        [Test]
        public void TestConstructing_WhenValueGiven_ThenToStringReturnsSameValue()
        {
            var password = new Password("asdgasdgasgd");

            Assert.That(password.ToString(), Is.EqualTo("asdgasdgasgd"));
        }
    }
}